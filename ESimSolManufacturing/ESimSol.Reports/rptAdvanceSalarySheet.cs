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
    public class rptAdvanceSalarySheet
    {
        #region Declaration
        int _nColumns;
        Document _oDocument;
        double deptWiseTotal, GrandTotal;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(8);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        List<EmployeeAdvanceSalary> _oEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();

        #endregion

        public byte[] PrepareReport(EmployeeAdvanceSalary EmployeeAdvanceSalary, List<SalarySheetSignature> oSalarySheetSignatures)
        {
            _oEmployeeAdvanceSalarys = EmployeeAdvanceSalary.EmployeeAdvanceSalarys;
            _oCompany = EmployeeAdvanceSalary.Company;
            
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 60f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 7;
            oPDFWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 32f, 80f, 150f, 150f, 70f, 100f, 100f, 160f });

            #endregion
            _nColumns = 8;
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Arial Black", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Advance Salary Sheet", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
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

            #endregion


            #region headerColumn

            float nHeight = 25;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ID", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Attendance Days", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross Amount", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Amount", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Cash", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Bank", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Account No.", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            int flag = 0;
            int counter = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            if (_oEmployeeAdvanceSalarys.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("There is no data to print !!", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                _oEmployeeAdvanceSalarys = _oEmployeeAdvanceSalarys.OrderBy(x => x.DepartmentName).ToList();
                while (_oEmployeeAdvanceSalarys.Count > 0)
                {
                    counter++;
                    List<EmployeeAdvanceSalary> oTempEmployeeAdvanceSalarys = new List<EmployeeAdvanceSalary>();
                    oTempEmployeeAdvanceSalarys = _oEmployeeAdvanceSalarys.Where(x => x.DepartmentID == _oEmployeeAdvanceSalarys[0].DepartmentID).OrderBy(x => x.Code).ToList();
                    //oTempDAs.AddRange(oTempAttendanceDailys);

                    if (oTempEmployeeAdvanceSalarys.Count > 0)
                    {
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        this.PrintHeader();
                    }
                    
                    PrintAttendanceDaily(oTempEmployeeAdvanceSalarys);
                    _oEmployeeAdvanceSalarys.RemoveAll(x => x.DepartmentID == oTempEmployeeAdvanceSalarys[0].DepartmentID);

                }

                _oFontStyle = FontFactory.GetFont("Arial Black", 13f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1; _oPdfPCell.Colspan = 6;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GrandTotal.ToString("#,##0"), _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion
        float nHeight = 23;
        private void PrintAttendanceDaily(List<EmployeeAdvanceSalary> oEAS)
        {
            int nCount = 0;
            deptWiseTotal = 0.0;
            int RowCount = 0;

            _oFontStyle = FontFactory.GetFont("Arial Black", 8f, iTextSharp.text.Font.BOLD); 
            _oPdfPCell = new PdfPCell(new Phrase(oEAS[0].BUName + ", " + oEAS[0].LocationName + ", " + oEAS[0].DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 25; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            foreach (EmployeeAdvanceSalary oItem in oEAS)
            {
                nCount++;
                RowCount++;
                _oFontStyle = FontFactory.GetFont("Arial Black", 9f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.BorderWidth = 1; _oPdfPCell.FixedHeight = nHeight;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Code, _oFontStyle)); _oPdfPCell.BorderWidth = 1; _oPdfPCell.FixedHeight = nHeight;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DesignationName, _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentDays.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GrossSalary.ToString("#,##0"), _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Arial Black", 13f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.NetAmount.ToString("#,##0"), _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Arial Black", 9f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                deptWiseTotal += oItem.NetAmount;

                if (RowCount % 17 == 0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader();
                }                
            }
            _oFontStyle = FontFactory.GetFont("Arial Black", 13f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1; _oPdfPCell.Colspan = 6;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(deptWiseTotal.ToString("#,##0"), _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.BorderWidth = 1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            GrandTotal += deptWiseTotal;
        }
    }

}

