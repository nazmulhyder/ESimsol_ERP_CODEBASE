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
using System.Linq;
using ICS.Core.Framework;

namespace ESimSol.Reports
{
    public class rptTimeCard_F05V2
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceDailyV2 _oAttendanceDailyV2 = new AttendanceDailyV2();
        List<AttendanceDailyV2> _oAttendanceDailyV2s = new List<AttendanceDailyV2>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        #endregion

        public byte[] PrepareReport(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, string StartDate, string EndDate)
        {
            _oAttendanceDailyV2s = oAttendanceDailyV2s;
            _oCompany = oCompany;

            _dstartDate = Convert.ToDateTime(StartDate);
            _dEndDate = Convert.ToDateTime(EndDate);

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 200f, 200f, 200f, 200f });
            #endregion

            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            if (_oAttendanceDailyV2s.Count > 0)
            {
                while (_oAttendanceDailyV2s.Count > 0)
                {
                    List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                    oADs = _oAttendanceDailyV2s.Where(x => x.EmployeeID == _oAttendanceDailyV2s[0].EmployeeID).ToList();
                    _oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);
                    EachEmployee(oADs);
                }
            }
        }
        private void EachEmployee(List<AttendanceDailyV2> oADs)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (oADs.Count > 0)
            {

                _oPdfPCell = new PdfPCell(GetTimeCard(oADs)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 8;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(GetFooterTable(oADs)); _oPdfPCell.Colspan = 8; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPTable.CompleteRow();
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
        }
        #endregion
        int nFixedHeight = 0;
        int nBorderWidth = 1;
        int nCount = 0;
        public PdfPTable GetTimeCard(List<AttendanceDailyV2> oAttendanceDailyV2s)
        {
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 65f, 65f, 65f, 65f, 65f, 65f, 65f, 65f });

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 400f });
            PdfPCell oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oAttendanceDailyV2s[oAttendanceDailyV2s.Count - 1].BUName, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oAttendanceDailyV2s[oAttendanceDailyV2s.Count - 1].BUAddress, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            oPdfPCell = new PdfPCell(oPdfPTableHeader);
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" "));
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 7;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Job Card Report From " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"), _oFontStyle));
            oPdfPCell.Colspan = 8;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" "));
            oPdfPCell.Colspan = 8;
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 7;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            nFixedHeight = 15;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Employee ID", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDailyV2s[0].EmployeeCode, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 3; oPdfPCell.BorderWidth = nBorderWidth;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Colspan = 2; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);



            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDailyV2s[0].JoiningDateInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDailyV2s[0].EmployeeName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Floor Name", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Colspan = 2; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDailyV2s[0].LocationName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDailyV2s[0].DesignationName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 3; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Colspan = 2; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 8; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            nFixedHeight = 16;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Entry Time", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Exit Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Exit Time", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //Day of week

            oPdfPCell = new PdfPCell(new Phrase("Duration", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Overtime", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthBottom = nBorderWidth;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            nCount = 0;

            dEndDate = dEndDate.AddDays(1);
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                string nameOfDay = dstartDate.ToString("ddd");
                nCount++;
                AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                oTempAttendanceDailyV2 = oAttendanceDailyV2s.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                //Date
                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                if (oTempAttendanceDailyV2!=null)
                {
                    //Entry Time
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Exit Date
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.OutTime.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Exit Time
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Duration
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.TotalWorkingHourSt, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    //Overtime
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.OverTimeInMinuteHourSt, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Status

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.AttStatusInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Shift
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                }

                else
                {
                    //Entry Time
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Exit Date
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Exit Time
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Duration
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    //Overtime
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Status

                    oPdfPCell = new PdfPCell(new Phrase("No Record Found", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Shift
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                }

                dstartDate = dstartDate.AddDays(1);
            }

           

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 8; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;

        }

        public PdfPTable GetFooterTable(List<AttendanceDailyV2> oAttendanceDailyV2s)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 120f, 120f, 100f });

            PdfPTable oLeftPdfPTable = new PdfPTable(2);
            oLeftPdfPTable.SetWidths(new float[] { 60f, 60f });

            PdfPTable oRightPdfPTable = new PdfPTable(2);
            oRightPdfPTable.SetWidths(new float[] { 60f, 60f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total Days: " + nCount, _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total Over Time: " + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "OT"), _oFontStyle)); oPdfPCell.BorderWidth = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Punch Day", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "P",false), _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Absent Days", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "A",false), _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Weekend Day", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "Off",false), _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("HoliDays", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "HD",false), _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);



            string sLeaveName = "";
            string[] LeaveDetails;
            List<AttendanceDailyV2> oAttendanceDailyV2S = new List<AttendanceDailyV2>();
            oAttendanceDailyV2S = oAttendanceDailyV2s.Where(x => x.LeaveName != "").GroupBy(p => p.LeaveName).Select(g => g.First()).ToList();
            sLeaveName = string.Join(",", oAttendanceDailyV2S.Select(x => x.LeaveName));
            LeaveDetails = sLeaveName.Split(',');

            double nTotalLeave = 0;

            foreach (string sLN in LeaveDetails)
            {
                if (sLN != "")
                {
                    oAttendanceDailyV2S = oAttendanceDailyV2s.Where(x => x.LeaveName != "" && x.LeaveName == sLN).ToList();

                    oPdfPCell = new PdfPCell(new Phrase(sLN, _oFontStyle)); oPdfPCell.BorderWidth = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oAttendanceDailyV2S.Count > 0 ? ": " + oAttendanceDailyV2S.Count().ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

                    nTotalLeave += oAttendanceDailyV2S.Count;
                }
            }


            oPdfPCell = new PdfPCell(oLeftPdfPTable); oPdfPCell.BorderWidth = 1;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(oRightPdfPTable); oPdfPCell.BorderWidth = 1;
            oPdfPTable.AddCell(oPdfPCell);
            int nTLeave = (int)nTotalLeave;
            int nPresent =Convert.ToInt32(AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "P", false));
            int nAbsent = oAttendanceDailyV2s.Where(x => x.AttStatusInString == "A").ToList().Count();
            int nDayOff = oAttendanceDailyV2s.Where(x => x.AttStatusInString == "Off").ToList().Count();
            int nHoliDay = oAttendanceDailyV2s.Where(x => x.AttStatusInString == "HD").ToList().Count();
            int GTotal=nTLeave+nPresent+nDayOff+nHoliDay;
            oPdfPCell = new PdfPCell(new Phrase("Total : " + GTotal.ToString(), _oFontStyle)); oPdfPCell.BorderWidth = 1;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
    }
}
