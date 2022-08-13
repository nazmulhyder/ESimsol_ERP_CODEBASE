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
    public class rptTimeCard_F6_1
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(8);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceDaily_ZN _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        List<AttendanceDaily_ZN> _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        bool bNewOrNot = false;
        bool _isComp = true;

        #endregion

        public byte[] PrepareReport(AttendanceDaily_ZN oAttendanceDaily_ZN)
        {
            _oAttendanceDaily_ZN = oAttendanceDaily_ZN;
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs;
            _oCompany = oAttendanceDaily_ZN.Company;

            _dstartDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[0]);
            _dEndDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[1]);

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(842, 595), 0f, 0f, 0f, 0f);//LANDSCAPE
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);


            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 7;
            oPDFWriter.PageEvent = PageEventHandler;

            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 65f, 65f, 65f, 65f, 65f, 65f, 65f, 65f});
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            if (_oAttendanceDaily_ZNs.Count > 0)
            {
                while (_oAttendanceDaily_ZNs.Count > 0)
                {
                    List<AttendanceDaily_ZN> oADs = new List<AttendanceDaily_ZN>();
                    oADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).ToList();
                    _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);
                    EachEmployee(oADs);
                }
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to Print !", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 8;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
        }
        private void EachEmployee(List<AttendanceDaily_ZN> oADs)
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

        double nPresent = 0;
        double nAbsent = 0;
        double nLeave = 0;
        double nDayOff = 0;
        double nHoliDay = 0;
        double nLateDays = 0;
        int nLateHrs = 0;
        double nEarlyDays = 0;
        int nEarlyHrs = 0;
        int nTotalOT = 0;
        string LeaveStatusDetails;
        int nCount = 0;
        int nFixedHeight = 0;
        int nAttCount = 0;
        int nPaidLeave = 0;

        int nBorderWidth = 1;
        public PdfPTable GetTimeCard(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 65f, 65f, 65f, 65f, 65f, 65f, 65f, 65f});

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 400f});
            PdfPCell oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[oAttendanceDaily_ZNs.Count - 1].BUName, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.Colspan = 2;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[oAttendanceDaily_ZNs.Count - 1].BUAddress, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            oPdfPCell = new PdfPCell(oPdfPTableHeader);
            oPdfPCell.Colspan =8;
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
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].EmployeeCode, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 3; oPdfPCell.BorderWidth = nBorderWidth;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Colspan = 2; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            //New Join
            if ((oAttendanceDaily_ZNs[0].JoiningDate >= dstartDate) && (oAttendanceDaily_ZNs[0].JoiningDate <= dEndDate))
            {
                bNewOrNot = true;
            }


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].JoiningDateInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].EmployeeName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Floor Name", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Colspan = 2; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].LocationName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].DesignationName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 3; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.Border = 0;
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

            nPresent = 0;
            nAbsent = 0;
            nLeave = 0;
            nDayOff = 0;
            nHoliDay = 0;
            nCount = 0;
            nLateDays = 0;
            nLateHrs = 0;
            nTotalOT = 0;
            nEarlyDays = 0;
            nEarlyHrs = 0;
            LeaveStatusDetails = "";
            nAttCount = 0;
            nPaidLeave = 0;

            dEndDate = dEndDate.AddDays(1);
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                string nameOfDay = dstartDate.ToString("ddd");
                nCount++;
                List<AttendanceDaily_ZN> oTempAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
                oTempAttendanceDaily_ZNs = oAttendanceDaily_ZNs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList();

                if (oTempAttendanceDaily_ZNs.Count > 0 && dstartDate >= oTempAttendanceDaily_ZNs[0].JoiningDate)
                {
                    for (int i = 0; i < oTempAttendanceDaily_ZNs.Count; i = i + 1)
                    {
                        string Status = "";
                        if (oTempAttendanceDaily_ZNs[i].InTimeInString_12hr == "-" && oTempAttendanceDaily_ZNs[i].OutTimeInString_12hr == "-" && oTempAttendanceDaily_ZNs[i].IsOSD == false) 
                        {
                            if (oTempAttendanceDaily_ZNs[i].IsDayOff == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                //Date
                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

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
                                Status = "Off";

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                Status = "HD";

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsLeave == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nLeave++;
                                if (oTempAttendanceDaily_ZNs[i].IsUnPaid == false) { nPaidLeave++; }
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                Status = "A";

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nAbsent++;
                            }
                        }
                        else
                        {
                            if (oTempAttendanceDaily_ZNs[i].IsDayOff == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].InTimeInString_12hr != "-") ? oTempAttendanceDaily_ZNs[i].InTimeInString_12hr : "-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].OutTimeInDateFormat != "-") ? oTempAttendanceDaily_ZNs[i].OutTimeInDateFormat : "-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].OutTimeInString_12hr != "-") ? oTempAttendanceDaily_ZNs[i].OutTimeInString_12hr : "-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].TotalWorkingHourInMinute > 0) ? (oTempAttendanceDaily_ZNs[i].TotalWorkingHourInMinute / 60) + ":" + (oTempAttendanceDaily_ZNs[i].TotalWorkingHourInMinute % 60).ToString("00") : "-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].OverTimeInMinute > 0) ? (oTempAttendanceDaily_ZNs[i].OverTimeInMinute / 60) + ":" + (oTempAttendanceDaily_ZNs[i].OverTimeInMinute % 60).ToString("00") : "-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                Status = "Off";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                Status = "HD";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsLeave == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                oPdfPTable.CompleteRow();
                                nLeave++;
                                if (oTempAttendanceDaily_ZNs[i].IsUnPaid == false) { nPaidLeave++; }
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsOSD == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString_12hr, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString_12hr, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].TotalWorkingHourInMinute), _oFontStyle)); oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                if (oTempAttendanceDaily_ZNs[i].IsOSD)
                                {
                                    Status += "OSD";
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                oPdfPTable.CompleteRow();
                                nPresent++;
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd/MM/yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthLeft = nBorderWidth; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString_12hr, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInDateFormat, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString_12hr, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                //Duration
                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].TotalWorkingHourInMinute / 60) + ":" + (oTempAttendanceDaily_ZNs[i].TotalWorkingHourInMinute % 60).ToString("00"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime
                                oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZNs[i].OverTimeInMinute / 60) + ":" + (oTempAttendanceDaily_ZNs[i].OverTimeInMinute % 60).ToString("00"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                Status = "P";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }
                                if (oTempAttendanceDaily_ZNs[i].LateArrivalMinute > 0)
                                {
                                    Status += ",Late";
                                    nLateDays++;
                                    nLateHrs += oTempAttendanceDaily_ZNs[i].LateArrivalMinute;
                                }
                                if (oTempAttendanceDaily_ZNs[i].EarlyDepartureMinute > 0)
                                {
                                    Status += ",Early";
                                    nEarlyDays++;
                                    nEarlyHrs += oTempAttendanceDaily_ZNs[i].EarlyDepartureMinute;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Shift
                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidthBottom = oPdfPCell.BorderWidthRight = nBorderWidth; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                oPdfPTable.CompleteRow();
                                nPresent++;
                            }
                        }
                        //if (oTempAttendanceDaily_ZNs[i].OverTimeInMinute > 0)
                        //{
                        //    nTotalOT += oTempAttendanceDaily_ZNs[i].OverTimeInMinute;
                        //}

                    }
                    nAttCount++;
                }
                dstartDate = dstartDate.AddDays(1);
            }

            string sLeaveName = "";
            string[] LeaveDetails;
            List<AttendanceDaily_ZN> oAttendanceDs = new List<AttendanceDaily_ZN>();
            oAttendanceDs = oAttendanceDaily_ZNs.Where(x => x.LeaveName != "").GroupBy(p => p.LeaveName).Select(g => g.First()).ToList();
            sLeaveName = string.Join(",", oAttendanceDs.Select(x => x.LeaveName));
            LeaveDetails = sLeaveName.Split(',');


            foreach (string sLN in LeaveDetails)
            {
                if (sLN != "")
                {
                    oAttendanceDs = oAttendanceDaily_ZNs.Where(x => x.LeaveName != "" && x.LeaveName == sLN).ToList();
                    LeaveStatusDetails += sLN + "=" + oAttendanceDs.Count + ",";
                }
            }
            if (LeaveStatusDetails != "") { LeaveStatusDetails = LeaveStatusDetails.Remove(LeaveStatusDetails.Length - 1); }


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 8; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;

        }

        public PdfPTable GetFooterTable(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 120f, 120f, 100f});

            PdfPTable oLeftPdfPTable = new PdfPTable(2);
            oLeftPdfPTable.SetWidths(new float[] { 60f, 60f });

            PdfPTable oRightPdfPTable = new PdfPTable(2);
            oRightPdfPTable.SetWidths(new float[] { 60f, 60f });

            //Resign need to calculate
            //double nTotalWorkingDay = ((_dEndDate - _dstartDate).TotalDays + 1) - nDayOff - nHoliDay -(bNewOrNot==true ?1:0);

            double nTotalWorkingDay = nAttCount - nDayOff - nHoliDay - (bNewOrNot == true ? 1 : 0);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total Days: " + nTotalWorkingDay, _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            int nTotalOTClaculate = oAttendanceDaily_ZNs.Sum(x=>x.OverTimeInMinute);
            oPdfPCell = new PdfPCell(new Phrase("Total Over Time: " + (nTotalOTClaculate / 60) + ":" + (nTotalOTClaculate % 60).ToString("00"), _oFontStyle)); oPdfPCell.BorderWidth = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.BorderWidth = 0; oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Punch Day", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nPresent > 0 ? ": " + nPresent.ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Absent Days", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nAbsent > 0 ? ": " + nAbsent.ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Weekend", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nDayOff > 0 ? ": " + nDayOff.ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("HoliDays", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nHoliDay > 0 ? ": " + nHoliDay.ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);
            
            //oLeftPdfPTable.CompleteRow();


            //List<AttendanceDaily_ZN> oAttendanceDs = new List<AttendanceDaily_ZN>();
            //oAttendanceDs = _oAttendanceDaily_ZNs.Where(x => x.LeaveName != "").GroupBy(p => p.LeaveName).Select(g => g.First()).ToList();


            string sLeaveName = "";
            string[] LeaveDetails;
            List<AttendanceDaily_ZN> oAttendanceDs = new List<AttendanceDaily_ZN>();
            oAttendanceDs = oAttendanceDaily_ZNs.Where(x => x.LeaveName != "").GroupBy(p => p.LeaveName).Select(g => g.First()).ToList();
            sLeaveName = string.Join(",", oAttendanceDs.Select(x => x.LeaveName));
            LeaveDetails = sLeaveName.Split(',');

            double nTotalLeave = 0;

            foreach (string sLN in LeaveDetails)
            {
                if (sLN != "")
                {
                    oAttendanceDs = oAttendanceDaily_ZNs.Where(x => x.LeaveName != "" && x.LeaveName == sLN).ToList();

                    oPdfPCell = new PdfPCell(new Phrase(sLN, _oFontStyle)); oPdfPCell.BorderWidth = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oAttendanceDs.Count > 0 ? ": " + oAttendanceDs.Count().ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

                    nTotalLeave += oAttendanceDs.Count;
                }
            }

            //foreach (var oItem in oAttendanceDs) 
            //{
            //    oPdfPCell = new PdfPCell(new Phrase(oItem.LeaveName, _oFontStyle)); oPdfPCell.BorderWidth = nBorderWidth;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

            //    int nVal= _oAttendanceDaily_ZNs.Where(x => x.LeaveName != "" && x.LeaveName == oItem.LeaveName).ToList().Count();
            //    oPdfPCell = new PdfPCell(new Phrase(nVal > 0 ? ": " + nVal.ToString() : ": -", _oFontStyle)); oPdfPCell.BorderWidth = nBorderWidth;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);
            //}

            oPdfPCell = new PdfPCell(oLeftPdfPTable); oPdfPCell.BorderWidth = 1;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(oRightPdfPTable); oPdfPCell.BorderWidth = 1;
            oPdfPTable.AddCell(oPdfPCell);

            double nGrandTotal = nTotalLeave + nPresent + nAbsent + nDayOff + nHoliDay;
            oPdfPCell = new PdfPCell(new Phrase("Total = " + nGrandTotal, _oFontStyle)); oPdfPCell.BorderWidth = 1;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }

    }

}
