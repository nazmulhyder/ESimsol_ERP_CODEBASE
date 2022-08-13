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
    public class ExportBillSelectedField
    {
        public ExportBillSelectedField()
        {
            FieldName = "";
            Caption = "";
        }
        public string FieldName { get; set; }
        public string Caption { get; set; }
    }
    public class rptExportBillReport
    {
        #region Declaration
        int _nColumns = 0;
        int _nPageWidth = 0;
        int _npageHeight = 550;
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable;
        //= new PdfPTable(9)
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<ExportBillReport> _oExportBillReports = new List<ExportBillReport>();
        ExportBillSelectedField _oExportBillSelectedField = new ExportBillSelectedField();
        List<ExportBillSelectedField> _oExportBillSelectedFields = new List<ExportBillSelectedField>();
        string[] _sFieldNames;
        string[] _sCaptions;
        #endregion

        public byte[] PrepareReport(List<ExportBillReport> oExportBillReports, BusinessUnit oBusinessUnit, Company oCompany,string sHeader, string sExportBillFieldST)
        {

            _oExportBillReports = oExportBillReports;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _sFieldNames = sExportBillFieldST.Split('~')[0].Split(',');
            _sCaptions = sExportBillFieldST.Split('~')[1].Split(',');

            for (int k = 0; k < _sFieldNames.Length; k++)
            {
                _oExportBillSelectedField = new ExportBillSelectedField();
                _oExportBillSelectedField.FieldName = _sFieldNames[k];
                _oExportBillSelectedField.Caption = _sCaptions[k];
                _oExportBillSelectedFields.Add(_oExportBillSelectedField);
            }

            #region Page Setup
            _nColumns = _sFieldNames.Length + 1;

            float[] tablecolumns = new float[_nColumns];

            if (_nColumns <= 5)
            {
                _nPageWidth = 500;
                tablecolumns[0] = 15f;

            }
            else
            {
                _nPageWidth = 90 * (_nColumns);
                tablecolumns[0] = 15f;

            }

            for (int i = 1; i < _nColumns; i++)
            {
                tablecolumns[i] = _nPageWidth / _nColumns;
            }


            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            //_oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            #endregion

            this.PrintHeader(sHeader);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 6;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sHeader)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 190f, 280f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(20f, 15f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                //oPdfPCellHearder.PaddingBottom = 8;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));

            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            oPdfPCellHearder.Colspan = _nColumns;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            for (int k = 0; k < _sCaptions.Length; k++)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_sCaptions[k], _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();

            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            foreach (ExportBillReport oEBItem in _oExportBillReports)
            {
                nCount++;
                //string[] sColumns = { "ExportLCNo,ApplicantName,BankName_Advice", "BankName_Nego", "ExportBillReportNo", "AmountSt", "StateSt", "StartDateSt", "SendToPartySt", "RecdFromPartySt", "SendToBankDateSt", "RecedFromBankDateSt", "LDBCDateSt", "LDBCNo", "AcceptanceDateSt", "MaturityReceivedDateSt", "MaturityDateSt", "DiscountedDateSt", "RelizationDateSt", "BankFDDRecDateSt", "EncashmentDateSt" };
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //for (int k = 0; k < sColumns.Length; k++)
                //{
                if (_oExportBillSelectedFields.Count > 0)
                {
                    foreach (ExportBillSelectedField oItem in _oExportBillSelectedFields)
                    {

                        if (oItem.FieldName == "ExportLCNo")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.ExportLCNo, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "ApplicantName")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.ApplicantName, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "BankName_Advice")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.BankName_Advice, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "BankName_Nego")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.BankName_Nego, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "ExportBillNo")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.ExportBillNo, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "AmountSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.AmountSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }

                        else if (oItem.FieldName == "StateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.StateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "LCRecivedDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.LCRecivedDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "StartDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.StartDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "SendToPartySt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.SendToPartySt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "RecdFromPartySt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.RecdFromPartySt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "RecdFromPartySt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.RecdFromPartySt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "SendToBankDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.SendToBankDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "RecedFromBankDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.RecedFromBankDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "LDBCDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.LDBCDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "LDBCNo")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.LDBCNo, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "AcceptanceDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.AcceptanceDateStr, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "MaturityReceivedDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.MaturityReceivedDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "MaturityDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.MaturityDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "DiscountedDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.DiscountedDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "RelizationDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.RelizationDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "BankFDDRecDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.BankFDDRecDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "EncashmentDateSt")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.EncashmentDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else if (oItem.FieldName == "MasterLCNos")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oEBItem.MasterLCNos, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }

                    }
                }
                else
                {

                }
                //}

                _oPdfPTable.CompleteRow();

            }
        }

        #endregion
    }

}
