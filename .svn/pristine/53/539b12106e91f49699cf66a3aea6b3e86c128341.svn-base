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
    public class rptTimeCard_Worker
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(10);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceDaily_ZN _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        List<AttendanceDaily_ZN> _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        int _nTotalOT = 0;
        int _nCount = 0;
        int _nFixedHeight = 0;

        #endregion

        public byte[] PrepareReport(AttendanceDaily_ZN oAttendanceDaily_ZN)
        {
            _oAttendanceDaily_ZN = oAttendanceDaily_ZN;
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs;
            _oLeaveHeads = oAttendanceDaily_ZN.LeaveHeads;
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

            _oPdfPTable.SetWidths(new float[] { 65f,35f,40f, 65f, 50f, 65f, 65f, 65f, 65f, 65f });
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
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to Print !", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 10;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
        }
        private void EachEmployee(List<AttendanceDaily_ZN> oADs)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (oADs.Count > 0)
            {

                _oPdfPCell = new PdfPCell(GetTimeCard(oADs)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 10;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(GetFooterTable(oADs)); _oPdfPCell.Colspan = 10; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPTable.CompleteRow();
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
        }
        #endregion

        
        int nLateHrs = 0;
        int nFixedHeight = 0;
       

        int nBorderWidth = 1;
        public PdfPTable GetTimeCard(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOX;
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 65f, 35f, 40f, 65f, 50f, 65f, 65f, 65f, 65f, 65f });

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
            oPdfPCell.Colspan =10;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" "));
            oPdfPCell.Colspan = 10;
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 7;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Report Basic Info
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("JOB CARD REPORT FROM " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"), _oFontStyle));
            oPdfPCell.Colspan = 10;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" "));
            oPdfPCell.Colspan = 10;
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 7;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            nFixedHeight = 15;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Employee ID", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;  oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].EmployeeCode, _oFontStyle)); oPdfPCell.Border = 15; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 4; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;   oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].JoiningDateInString, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; ; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].EmployeeName, _oFontStyle)); oPdfPCell.Colspan = 4; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;  oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].DepartmentName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;  oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].DesignationName, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 4;  oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Floor Name", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;  oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[0].LocationName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 15; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Colspan = 10;  oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion



            nFixedHeight = 16;

            #region Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Day", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Out Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Out", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //Day of week

            oPdfPCell = new PdfPCell(new Phrase("Duration", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Overtime", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion


            _nCount = 0; _nTotalOT = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            dEndDate = dEndDate.AddDays(1);

            #region Body Data Information 
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                _nCount++;
                AttendanceDaily_ZN oTempAttendanceDaily_ZN = new AttendanceDaily_ZN();
                oTempAttendanceDaily_ZN = oAttendanceDaily_ZNs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                //Date
                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                //Day
                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("ddd"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.Border = 15;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                if (oTempAttendanceDaily_ZN != null && oTempAttendanceDaily_ZN.AttendanceID > 0)
                {
                    //In
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    
                    //Out Date 
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.OutTimeInDateFormat, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Out
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    
                    //Duration
                    oPdfPCell = new PdfPCell(new Phrase((oTempAttendanceDaily_ZN.TotalWorkingHourInMinute > 0) ? (oTempAttendanceDaily_ZN.TotalWorkingHourInMinute / 60) + "h " + (oTempAttendanceDaily_ZN.TotalWorkingHourInMinute % 60) + "m" : "-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Overtime
                    oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZN.OverTimeInMinute), _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    _nTotalOT = _nTotalOT + oTempAttendanceDaily_ZN.OverTimeInMinute;

                    //Shift
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    #region Total Late Hours
                    if (oTempAttendanceDaily_ZN.LateArrivalMinute > 0)
                    {
                        nLateHrs += oTempAttendanceDaily_ZN.LateArrivalMinute;
                    }
                    #endregion

                    //Status
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.AttStatusInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    //Remarks
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.Remark, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                }
                oPdfPTable.CompleteRow();
                dstartDate = dstartDate.AddDays(1);
            }



            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 10; oPdfPCell.FixedHeight = nFixedHeight; oPdfPCell.BorderWidth = nBorderWidth;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
            #endregion
        }
      

   
        #region Footer Information
        public PdfPTable GetFooterTable(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 40f, 245f, 195f});

            PdfPTable oLeftPdfPTable = new PdfPTable(2);
            oLeftPdfPTable.SetWidths(new float[] { 65f, 35f });

            PdfPTable oMidPdfPTable = new PdfPTable(4);
            oMidPdfPTable.SetWidths(new float[] { 65f, 50f, 65f, 65f });

            PdfPTable oRightPdfPTable = new PdfPTable(3);
            oRightPdfPTable.SetWidths(new float[] { 65f, 65f,65f });


            //Present
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Present", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            //int nPresent = this.GetCountedDays(oAttendanceDaily_ZNs, "P");
            int nPresent = this.GetCountedDaysForP(oAttendanceDaily_ZNs, "P");
            oPdfPCell = new PdfPCell(new Phrase(nPresent > 0 ? nPresent.ToString() : "", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            //Absent
            oPdfPCell = new PdfPCell(new Phrase("Absent", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            int nAbsent = this.GetCountedDays(oAttendanceDaily_ZNs, "A");
            oPdfPCell = new PdfPCell(new Phrase(nAbsent > 0 ? nAbsent.ToString() : "", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            //Holiday
            oPdfPCell = new PdfPCell(new Phrase("Holiday", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            int nHoliDay = this.GetCountedDays(oAttendanceDaily_ZNs, "HD");
            oPdfPCell = new PdfPCell(new Phrase(nHoliDay > 0 ? nHoliDay.ToString() : "", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            //Off day
            oPdfPCell = new PdfPCell(new Phrase("Off day", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);

            int nDayOff = this.GetCountedDays(oAttendanceDaily_ZNs, "Off");
            oPdfPCell = new PdfPCell(new Phrase(nDayOff > 0 ? nDayOff.ToString() : "", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oLeftPdfPTable.AddCell(oPdfPCell);
          

            oPdfPCell = new PdfPCell(oLeftPdfPTable);
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            List<AttendanceDaily_ZN> oAttendanceDs = new List<AttendanceDaily_ZN>();
            double nTotalLeave = 0;


            foreach (LeaveHead item in _oLeaveHeads)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(item.ShortName, _oFontStyle)); oPdfPCell.Border = 15;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oMidPdfPTable.AddCell(oPdfPCell);

                oAttendanceDs = oAttendanceDaily_ZNs.Where(x => x.LeaveName != "" && x.LeaveName == item.ShortName).ToList();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase(oAttendanceDs.Count > 0 ? oAttendanceDs.Count().ToString() : "0", _oFontStyle)); oPdfPCell.Border = 15;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oMidPdfPTable.AddCell(oPdfPCell);

                nTotalLeave += oAttendanceDs.Count;
            }
            oPdfPCell = new PdfPCell(new Phrase("Total Leave", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oMidPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nTotalLeave.ToString(), _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oMidPdfPTable.AddCell(oPdfPCell);

            double nTotalAttendance = nPresent + nAbsent + nHoliDay + nDayOff + nTotalLeave;
            oMidPdfPTable.CompleteRow();
            oPdfPCell = new PdfPCell(oMidPdfPTable); 
            oPdfPTable.AddCell(oPdfPCell);




            //Late Days
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Late Days", _oFontStyle)); oPdfPCell.Border = 15; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

            int nLateDays = this.GetCountedDays(oAttendanceDaily_ZNs, "Late");
            oPdfPCell = new PdfPCell(new Phrase(nLateDays > 0 ? nLateDays.ToString() : "0", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

            //Late Hours
            oPdfPCell = new PdfPCell(new Phrase("Late Hours", _oFontStyle)); oPdfPCell.Border = 15; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nLateHrs > 0 ? (nLateHrs / 60) + "h " + (nLateHrs % 60) + "m" : "0", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);
            
            //Total Attendance
            oPdfPCell = new PdfPCell(new Phrase("Total Attendance", _oFontStyle)); oPdfPCell.Border = 15; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nTotalAttendance > 0 ? nTotalAttendance.ToString() : "0", _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);
            
            //Total Overtime
            oPdfPCell = new PdfPCell(new Phrase("Total Overtime", _oFontStyle)); oPdfPCell.Border = 15; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);
            
            oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(_nTotalOT), _oFontStyle)); oPdfPCell.Border = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oRightPdfPTable.AddCell(oPdfPCell);

            oRightPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(oRightPdfPTable);
            oPdfPTable.AddCell(oPdfPCell);
            //double nGrandTotal = nTotalLeave + nPresent + nAbsent + nDayOff + nHoliDay;
            //oPdfPCell = new PdfPCell(new Phrase("Total = " + nGrandTotal, _oFontStyle)); oPdfPCell.BorderWidth = 1;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        #endregion

        private int GetCountedDays(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, string sDaySign)
        {
            int nCountedDays = 0;
            foreach (AttendanceDaily_ZN oItem in oAttendanceDaily_ZNs)
            {
                if (sDaySign == "Leave")
                {
                    if (oItem.IsLeave)
                    {
                        nCountedDays = nCountedDays + 1;
                    }
                }
                else
                {
                    string[] aDaysSigns = oItem.AttStatusInString.Split(',');
                    if (aDaysSigns != null && aDaysSigns.Length > 0)
                    {
                        for (int i = 0; i < aDaysSigns.Length; i++)
                        {
                            if (aDaysSigns[i] == sDaySign)
                            {
                                nCountedDays = nCountedDays + 1;
                            }
                        }
                    }

                }
            }
            return nCountedDays;
        }

        #region Count Present
        private int GetCountedDaysForP(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs, string sDaySign)
        {
            int nCountedDays = 0;
            foreach (AttendanceDaily_ZN oItem in oAttendanceDaily_ZNs)
            {
                int nIndex = -1;
                nIndex = (sDaySign == "P") ? oItem.AttStatusInString.IndexOf(sDaySign) : nIndex;
                if (nIndex == 0)
                {
                    nCountedDays = nCountedDays + 1;
                }
            }
            return nCountedDays;
        }
        #endregion
    }

}
