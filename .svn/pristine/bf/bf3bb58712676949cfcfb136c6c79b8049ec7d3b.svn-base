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
    public class rptMonthlyAttendance
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(13);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        MonthlyAttendanceReport _oMonthlyAttendanceReport = new MonthlyAttendanceReport();
        List<MonthlyAttendanceReport> _oMonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(MonthlyAttendanceReport oMonthlyAttendanceReport, List<BusinessUnit> oBusinessUnits)
        {
            _oMonthlyAttendanceReports = oMonthlyAttendanceReport.MonthlyAttendanceReports;
            _oMonthlyAttendanceReport = oMonthlyAttendanceReport;
            _oCompany = oMonthlyAttendanceReport.Company;
            _oBusinessUnits = oBusinessUnits;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = new List<string> { "Prepared By", "Approved By"};
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 10;
            oPDFWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 25f, 55f, 130f, 70f, 40f, 40f, 40f, 40f, 40f, 65f, 40f, 38f, 38f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(int nBusinessUnitID)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 160f, 255f });
            PdfPCell oPdfPCellHearder;

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == nBusinessUnitID).ToList();

            if (_oBusinessUnits.Count > 1 || _oBusinessUnits.Count == 0)
            {
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
                oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));

                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
                oPdfPCellHearder.Colspan = 2;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTableHeader);
                _oPdfPCell.Colspan = 13;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 13;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 10;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits[0].Name, _oFontStyle));

                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits[0].Address, _oFontStyle));
                oPdfPCellHearder.Colspan = 2;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTableHeader);
                _oPdfPCell.Colspan = 13;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 13;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 10;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Multiple Employee Time Card Details", _oFontStyle));
            _oPdfPCell.Colspan = 13;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 13;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oMonthlyAttendanceReport.ErrorMessage, _oFontStyle)); _oPdfPCell.Colspan = 13; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            #endregion

        }
        #endregion

        #region Report Body
        private void ColumnSetup()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Joining", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("WD", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PD", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Off", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("HD", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LD", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Half/Short L", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Absent", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NW", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        private void Body(List<MonthlyAttendanceReport> oMonthlyAttendanceReports)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0;
            foreach (MonthlyAttendanceReport oItem in oMonthlyAttendanceReports)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.JoiningDateInString, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.TotalWorkingDaySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PresentDaySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DayOFFSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.HoliDaySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LeaveSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LeaveHalfShortSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AbsentDaySt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.NoWorkSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OvertimeInhourSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                if (nCount % 35 == 0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader(oItem.BusinessUnitID);
                    this.ColumnSetup();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                }
            }

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 13; _oPdfPCell.FixedHeight = 50; _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void PrintBody()
        {

            _oMonthlyAttendanceReports = _oMonthlyAttendanceReports.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
            while (_oMonthlyAttendanceReports.Count > 0)
            {
                var oResults = _oMonthlyAttendanceReports.Where(x => x.BusinessUnitID == _oMonthlyAttendanceReports[0].BusinessUnitID && x.LocationName == _oMonthlyAttendanceReports[0].LocationName && x.DepartmentName == _oMonthlyAttendanceReports[0].DepartmentName).OrderBy(x => x.EmployeeCode).ToList();
                
                PrintHeader(oResults[0].BusinessUnitID);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oResults.FirstOrDefault().LocationName + ",Department : " + oResults.FirstOrDefault().DepartmentName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 13; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                this.ColumnSetup();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                this.Body(oResults);

                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();

                _oMonthlyAttendanceReports.RemoveAll(x => x.BusinessUnitID == oResults[0].BusinessUnitID && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);
            }
            
        }
        #endregion
    }
}
