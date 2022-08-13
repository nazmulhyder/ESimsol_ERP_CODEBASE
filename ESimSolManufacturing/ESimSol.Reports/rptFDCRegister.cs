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

    public class rptFDCRegister
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        FDCRegister _oFDCRegister = new FDCRegister();
        List<FDCRegister> _oFDCRegisters = new List<FDCRegister>();
        double dGrandTotal = 0.0;
        double dSubTotal = 0.0;
        double dDateWise = 0.0;
        double dPartyWise = 0.0;
        string ReportLayoutHeaderName = "";
        string ReportChallanDateString = "";
        #endregion

        #region Report Header
        private void PrintHeader()
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
            _oPdfPCell = new PdfPCell(new Phrase(ReportLayoutHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oFDCRegister.Params, _oFontStyle));
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
        private string MakeReportChallanDateHeader(string sString)
        {
            int nCount = 0;
            int cboChallanDate = 0;
            string ChallanStartDate = "";
            string ChallanEndDate = "";
            if (!String.IsNullOrEmpty(sString))
            {
                cboChallanDate = Convert.ToInt32(sString.Split('~')[nCount++]);
                ChallanStartDate = Convert.ToString(sString.Split('~')[nCount++]);
                ChallanEndDate = Convert.ToString(sString.Split('~')[nCount++]);
            }
            if(cboChallanDate == 0)
            {
                _oFDCRegister.Params = "";
            }
            if (cboChallanDate == 1)
            {
                _oFDCRegister.Params = "Challan Date is Equal To " + ChallanStartDate;
            }
            if (cboChallanDate == 2)
            {
                _oFDCRegister.Params = "Challan Date is Not Equal To " + ChallanStartDate;
            }
            if (cboChallanDate == 3)
            {
                _oFDCRegister.Params = "Challan Date is Greater Than " + ChallanStartDate;
            }
            if (cboChallanDate == 4)
            {
                _oFDCRegister.Params = "Challan Date is Smaller Than " + ChallanStartDate;
            }
            if (cboChallanDate == 5)
            {
                _oFDCRegister.Params = "Challan Date is Between " + ChallanStartDate + " to " + ChallanEndDate;
            }
            if (cboChallanDate == 6)
            {
                _oFDCRegister.Params = "Challan Date is Not Between " + ChallanStartDate + " to " + ChallanEndDate;
            }
            return _oFDCRegister.Params;
        }


        #region Date Wise
        public byte[] PrepareReportDateWise(Company oCompany, string sHeaderName, List<FDCRegister> oFDCRegisters, FDCRegister oFDCRegister)
        {
            _oCompany = oCompany;
            _oFDCRegisters = oFDCRegisters;
            _oFDCRegister = oFDCRegister;
            ReportLayoutHeaderName = sHeaderName;

            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842});
            #endregion
            this.MakeReportChallanDateHeader(oFDCRegister.Params);
            this.PrintHeader();
            this.PrintEmptyRow();
            this.HeaderTableDateWise();
            this.DataTableDateWise();
            this.GrandTotal();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void HeaderTableDateWise()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            oPdfPTable.SetWidths(SetWidth());
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("#SL" + Environment.NewLine + "No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Person" + Environment.NewLine + "Challan No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DO No" + Environment.NewLine + "DO Date", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Party Name" + Environment.NewLine + "Challan By", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product" + Environment.NewLine + "Code", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product" + Environment.NewLine + "Name", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Ref No" + Environment.NewLine + "Dispo No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product" + Environment.NewLine + "Construction ", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC No" + Environment.NewLine + "PO No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI" + Environment.NewLine + "No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Lot" + Environment.NewLine + "No", _oFontStyle));
            //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M" + Environment.NewLine + "Unit", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan" + Environment.NewLine + "Qty", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void DataTableDateWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            int nCount = 1;

            string sPreviousChallanDate = "";
            int nChallanID = 0;
            foreach (FDCRegister oFDCRegister in _oFDCRegisters)
            {
                PdfPTable oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
                oPdfPTable.SetWidths(SetWidth());
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                if (nChallanID != oFDCRegister.FDCID && nCount > 1)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(dSubTotal.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (sPreviousChallanDate != oFDCRegister.ChallanDateSt && nCount > 1)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = 0;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Challan Date Wise Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(dDateWise.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (sPreviousChallanDate != oFDCRegister.ChallanDateSt)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Challan Date: " + oFDCRegister.ChallanDateSt, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 13;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #region Span Data Row
                if (nChallanID != oFDCRegister.FDCID && nCount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nCount++.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.MKTPerson + Environment.NewLine + oFDCRegister.ChallanNo, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.DONo + Environment.NewLine + oFDCRegister.FDODateSt, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ContractorName + Environment.NewLine + oFDCRegister.DisburseByName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                #endregion

                #region Data Row

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ProductCode, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ProductName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.FabricNo + Environment.NewLine + oFDCRegister.ExeNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.Construction, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.LCNo + Environment.NewLine + oFDCRegister.SCNoFull, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.PINo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.LotNo, _oFontStyle));
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.MUName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.Qty.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion


                sPreviousChallanDate = oFDCRegister.ChallanDateSt;
                nChallanID = oFDCRegister.FDCID;
                dSubTotal = _oFDCRegisters.Where(x => x.FDCID.Equals(oFDCRegister.FDCID)).Sum(x => x.Qty);
                dDateWise = _oFDCRegisters.Where(x => x.ChallanDateSt.Equals(oFDCRegister.ChallanDateSt)).Sum(x => x.Qty);
                //nRowSpan = _oFDCRegisters.Where(x => x.FDCID.Equals(oFDCRegister.FDCID)).Count();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.Colspan = 1;
                _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion

        #region MKT Person Wise
        public byte[] PrepareReportMKTPersonWise(Company oCompany, string sHeaderName, List<FDCRegister> oFDCRegisters, FDCRegister oFDCRegister)
        {
            _oCompany = oCompany;
            _oFDCRegisters = oFDCRegisters;
            _oFDCRegister = oFDCRegister;
            ReportLayoutHeaderName = sHeaderName;

            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[]{842f});
            #endregion
            this.MakeReportChallanDateHeader(_oFDCRegister.Params);
            this.PrintHeader();
            this.PrintEmptyRow();
            this.HeaderTableMKTPWise();
            this.DataTableMKTPWise();
            this.GrandTotal();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void HeaderTableMKTPWise()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            oPdfPTable.SetWidths(SetWidth());
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("#SL" + Environment.NewLine+ "No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan Date" + Environment.NewLine + "Challan No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DO No" + Environment.NewLine + "DO Date", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Party Name" + Environment.NewLine + "Challan By", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Code", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product" + Environment.NewLine + "Name", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Ref No" + Environment.NewLine+ "Dispo No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Construction ", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC No" + Environment.NewLine+ "PO No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI" + Environment.NewLine + "No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Lot" + Environment.NewLine + "No", _oFontStyle));
            //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M" + Environment.NewLine + "Unit", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan" + Environment.NewLine + "Qty", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        
        public void DataTableMKTPWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 1;

            int nMKTPersonID = 0;
            int nChallanID = 0;
            int nRowSpan = 1;
            foreach (FDCRegister oFDCRegister in _oFDCRegisters)
            {
                PdfPTable oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
                oPdfPTable.SetWidths(SetWidth());

                if (nChallanID != oFDCRegister.FDCID && nCount > 1)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(dSubTotal.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (nMKTPersonID != oFDCRegister.MKTPersonID && nCount > 1)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = 0;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Marketing Person Wise Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(dDateWise.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (nMKTPersonID != oFDCRegister.MKTPersonID)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.MKTPerson, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 13;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #region Span Data Row

                if (nChallanID != oFDCRegister.FDCID && nCount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nCount++.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ChallanNo + Environment.NewLine + oFDCRegister.ChallanDateSt, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.DONo + Environment.NewLine + oFDCRegister.FDODateSt, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ContractorName + Environment.NewLine + oFDCRegister.DisburseByName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                #endregion

                #region Data Row

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ProductCode, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ProductName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.FabricNo + Environment.NewLine + oFDCRegister.ExeNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.Construction, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.LCNo + Environment.NewLine + oFDCRegister.SCNoFull, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.PINo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.LotNo, _oFontStyle));
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.MUName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.Qty.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion


                nMKTPersonID = oFDCRegister.MKTPersonID;
                nChallanID = oFDCRegister.FDCID;
                dSubTotal = _oFDCRegisters.Where(x => x.FDCID.Equals(oFDCRegister.FDCID)).Sum(x => x.Qty);
                dDateWise = _oFDCRegisters.Where(x => x.MKTPersonID.Equals(oFDCRegister.MKTPersonID)).Sum(x => x.Qty);
                nRowSpan = _oFDCRegisters.Where(x => x.FDCID.Equals(oFDCRegister.FDCID)).Count();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.Colspan = 1;
                _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion

        #region Party Wise
        public byte[] PrepareReportPartyWise(Company oCompany, string sHeaderName, List<FDCRegister> oFDCRegisters, FDCRegister oFDCRegister)
        { 
            _oCompany = oCompany;
            _oFDCRegisters = oFDCRegisters;
            _oFDCRegister = oFDCRegister;
            ReportLayoutHeaderName = sHeaderName;

            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(30f, 30f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[]{842});
            #endregion
            this.MakeReportChallanDateHeader(_oFDCRegister.Params);
            this.PrintHeader();
            this.PrintEmptyRow();
            this.HeaderTablePartyWise();
            this.DataTablePartyWise();
            this.GrandTotal();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void HeaderTablePartyWise()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            oPdfPTable.SetWidths(SetWidth());
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("#SL" + Environment.NewLine + "No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Chalan Date" + Environment.NewLine + "Challan No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DO No" + Environment.NewLine + "DO Date", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Person" + Environment.NewLine + "Challan By", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product" + Environment.NewLine + "Code", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product" + Environment.NewLine + "Name", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Ref No" + Environment.NewLine + "Dispo No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Construction ", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC No" + Environment.NewLine + "PO No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI" + Environment.NewLine + "No", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Lot" + Environment.NewLine + "No", _oFontStyle));
            //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M" + Environment.NewLine + "Unit", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan" + Environment.NewLine + "Qty", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void DataTablePartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 1;

            int nChallanID = 0;
            int nContractorID = 0;
            foreach (FDCRegister oFDCRegister in _oFDCRegisters)
            {
                PdfPTable oPdfPTable = new PdfPTable(12);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
                oPdfPTable.SetWidths(SetWidth());

                if (nChallanID != oFDCRegister.FDCID && nCount > 1)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Sub Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(dSubTotal.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (nContractorID != oFDCRegister.ContractorID && nCount > 1)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = 0;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Party Wise Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(dDateWise.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (nContractorID != oFDCRegister.ContractorID)
                {
                    PdfPTable oPdfPTableNew = new PdfPTable(12);
                    oPdfPTableNew.WidthPercentage = 100;
                    oPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oPdfPTableNew.SetWidths(SetWidth());
                    _oPdfPCell = new PdfPCell(new Phrase("Buyer/Party Name:" + oFDCRegister.ContractorName, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 13;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableNew.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(oPdfPTableNew);
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                    _oPdfPCell.Colspan = 1;
                    _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #region Span Data Row

                if (nChallanID != oFDCRegister.FDCID && nCount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nCount++.ToString(), _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ChallanDateSt + Environment.NewLine + oFDCRegister.ChallanNo, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.FDODateSt + Environment.NewLine + oFDCRegister.DONo, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.BuyerName + Environment.NewLine + oFDCRegister.DisburseByName, _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                #endregion

                #region Data Row

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ProductCode, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.ProductName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.FabricNo + Environment.NewLine + oFDCRegister.ExeNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.Construction, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.LCNo + Environment.NewLine + oFDCRegister.SCNoFull, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.PINo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.LotNo, _oFontStyle));
                //_oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.MUName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oFDCRegister.Qty.ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion


                nContractorID = oFDCRegister.ContractorID;
                nChallanID = oFDCRegister.FDCID;
                dSubTotal = _oFDCRegisters.Where(x => x.FDCID.Equals(oFDCRegister.FDCID)).Sum(x => x.Qty);
                dDateWise = _oFDCRegisters.Where(x => x.ContractorID.Equals(oFDCRegister.ContractorID)).Sum(x => x.Qty);
                //nRowSpan = _oFDCRegisters.Where(x => x.FDCID.Equals(oFDCRegister.FDCID)).Count();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                _oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                _oPdfPCell.Colspan = 1;
                _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        #endregion

        public void GrandTotal()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(SetWidth());
            
            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total:", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD))); _oPdfPCell.Colspan = 11;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            dGrandTotal = _oFDCRegisters.Sum(x => x.Qty);
            _oPdfPCell = new PdfPCell(new Phrase(dGrandTotal.ToString(), FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintEmptyRow()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[]{842});
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private float[] SetWidth()
        {
            return new float[] { 30, 80, 55, 90, 70, 130, 80, 90, 85, 70, 28, 45};
        }

    }
}

