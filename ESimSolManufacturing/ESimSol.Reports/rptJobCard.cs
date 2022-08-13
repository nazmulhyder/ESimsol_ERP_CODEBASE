﻿using System;
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
    public class rptJobCard
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceDaily_ZN _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        List<AttendanceDaily_ZN> _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        int _nTotalOT = 0;
        int _nCount = 0;
        double _dTemp = 0.00;
        double _dEmployeeShiftDurationInMin = 0.00;
        double _dEmployeeWorkingHour = 0.00;
        int _nFixedHeight = 0;
        #endregion

        public byte[] PrepareReport(AttendanceDaily_ZN oAttendanceDaily_ZN)
        {
            _oAttendanceDaily_ZN = oAttendanceDaily_ZN;
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs;
            _dTemp = _oAttendanceDaily_ZNs.Sum(x => x.TotalWorkingHourInMinute);
            _oCompany = oAttendanceDaily_ZN.Company;
            _oBusinessUnits = oAttendanceDaily_ZN.BusinessUnits;

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
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 200f, 200f, 200f, 200f });
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

                _oPdfPCell = new PdfPCell(GetTimeCard(oADs)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(GetFooterTable(oADs)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 40;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("_______________\nPREPARED BY", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


            }
        }
        #endregion


        public PdfPTable GetTimeCard(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 10f, 28f, 15f, 15f, 15f, 15f, 30f, 50f, 30f, 25f });

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[oAttendanceDaily_ZNs.Count - 1].BUName, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oAttendanceDaily_ZNs[oAttendanceDaily_ZNs.Count - 1].BUAddress, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            oPdfPCell = new PdfPCell(oPdfPTableHeader);
            oPdfPCell.Colspan = 10;
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
            oPdfPCell = new PdfPCell(new Phrase("Job Card", _oFontStyle));
            oPdfPCell.Colspan = 10;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("UNIT : " + (oAttendanceDaily_ZNs.Count > 0 ? oAttendanceDaily_ZNs[0].LocationName : ""), _oFontStyle));
            oPdfPCell.Colspan = 6;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase("Print Date : " + DateTime.Now.ToString("dd MMM yyyy HH:mm"), _oFontStyle));
            oPdfPCell.Colspan = 4;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 10; oPdfPCell.FixedHeight = 10;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _nFixedHeight = 15;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("CODE", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].EmployeeCode, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD); 
            oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL); oPdfPCell.Colspan = 3;
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].EmployeeName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell = new PdfPCell(new Phrase("DEPARTMENT", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3;oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL); oPdfPCell.FixedHeight = _nFixedHeight; 
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].DepartmentName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("DESIGNATION", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL); oPdfPCell.Colspan = 2;
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].DesignationName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("JOINING", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].JoiningDateInString, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD); 
            oPdfPCell = new PdfPCell(new Phrase("DATE RANGE", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2; oPdfPCell.FixedHeight = _nFixedHeight; oPdfPCell.Colspan = 3;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _nFixedHeight = 15;

            #region Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Late", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Out", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Early", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("T W Hour", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift Duration", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion


            _nCount = 0; _nTotalOT = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            

            dEndDate = dEndDate.AddDays(1);
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                _nCount++;
                AttendanceDaily_ZN oTempAttendanceDaily_ZN = new AttendanceDaily_ZN();
                oTempAttendanceDaily_ZN = oAttendanceDaily_ZNs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                if (oTempAttendanceDaily_ZN != null && oTempAttendanceDaily_ZN.AttendanceID > 0)
                {
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.LateArrivalHourSt, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.EarlyDepartureHrSt, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.TotalWorkingHourSt, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.ShiftName, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    string sShiftDuration = "";
                    //if (oTempAttendanceDaily_ZN.AttStatusInString.Contains("CL") || oTempAttendanceDaily_ZN.AttStatusInString.Contains("AL") || oTempAttendanceDaily_ZN.AttStatusInString.Contains("ML"))
                    if (oTempAttendanceDaily_ZN.IsLeave == true)
                    {
                        if(oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Full)
                        {
                            sShiftDuration = "0";
                        }
                        else if (oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Short)
                        {
                            TimeSpan tLeaveDuration = TimeSpan.FromMinutes(oTempAttendanceDaily_ZN.LeaveDuration);
                            TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                            span = span.Subtract(span - tLeaveDuration);
                            sShiftDuration = ((span.Hours > 0) ? span.Hours + "h" : "") + "  " + ((span.Minutes > 0) ? span.Minutes + "m" : "");
                            if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                            {
                                _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                            }
                        }
                        else if (oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Half)
                        {
                            TimeSpan tLeaveDuration = TimeSpan.FromMinutes(oTempAttendanceDaily_ZN.LeaveDuration);
                            TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                            span = span - tLeaveDuration;
                            sShiftDuration = ((span.Hours > 0) ? span.Hours + "h" : "") + "  " + ((span.Minutes > 0) ? span.Minutes + "m" : "");
                            if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                            {
                                _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                            }
                        }
                    }
                    else if (oTempAttendanceDaily_ZN.AttStatusInString.Contains("HD") || oTempAttendanceDaily_ZN.AttStatusInString.Contains("Off") || oTempAttendanceDaily_ZN.AttStatusInString.Contains("SL"))
                    {
                        if (oTempAttendanceDaily_ZN.InTimeInString == "-")
                        {
                            sShiftDuration = "0";
                        }
                        else
                        {
                            if (oTempAttendanceDaily_ZN.ShiftStartTime < oTempAttendanceDaily_ZN.ShiftEndTime)
                            {
                                TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                                sShiftDuration = ((span.Hours > 0) ? span.Hours + "h" : "") + "  " + ((span.Minutes > 0) ? span.Minutes + "m" : "");
                                if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                                {
                                    _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                                }
                            }
                            else if (oTempAttendanceDaily_ZN.ShiftStartTime > oTempAttendanceDaily_ZN.ShiftEndTime)
                            {
                                //int tMinuteDifference = (24*60) - ((oTempAttendanceDaily_ZN.ShiftStartTime.Hour*60)+oTempAttendanceDaily_ZN.ShiftStartTime.Minute) + ((oTempAttendanceDaily_ZN.ShiftEndTime.Hour*60)+oTempAttendanceDaily_ZN.ShiftEndTime.Minute);
                                TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                                sShiftDuration = ((span.Hours > 0) ? span.Hours + "h" : "") + "  " + ((span.Minutes > 0) ? span.Minutes + "m" : "");
                                if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                                {
                                    _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                                }
                            }
                        }
                    }
                    else if (oTempAttendanceDaily_ZN.ShiftStartTime < oTempAttendanceDaily_ZN.ShiftEndTime)
                    {
                        TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                        sShiftDuration = ((span.Hours > 0) ? span.Hours + "h" : "") + "  " + ((span.Minutes > 0) ? span.Minutes + "m" : "");
                        if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                        {
                            _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                        }
                    }
                    else if (oTempAttendanceDaily_ZN.ShiftStartTime > oTempAttendanceDaily_ZN.ShiftEndTime)
                    {
                        //int tMinuteDifference = (24*60) - ((oTempAttendanceDaily_ZN.ShiftStartTime.Hour*60)+oTempAttendanceDaily_ZN.ShiftStartTime.Minute) + ((oTempAttendanceDaily_ZN.ShiftEndTime.Hour*60)+oTempAttendanceDaily_ZN.ShiftEndTime.Minute);
                        TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                        sShiftDuration = ((span.Hours > 0) ? span.Hours + "h" : "") + "  " + ((span.Minutes > 0) ? span.Minutes + "m" : "");
                        if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                        {
                            _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                        }
                    }
                    oPdfPCell = new PdfPCell(new Phrase(sShiftDuration, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    if (oTempAttendanceDaily_ZN.AttStatusInString.Contains("CL"))
                    {
                        if (oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Short)
                        {
                            string str = oTempAttendanceDaily_ZN.AttStatusInString;
                            str = str.Replace("CL", "CL(S)");
                            oPdfPCell = new PdfPCell(new Phrase(str, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        }
                        else if(oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Half)
                        {
                            string str = oTempAttendanceDaily_ZN.AttStatusInString;
                            str = str.Replace("CL", "CL(H)");
                            oPdfPCell = new PdfPCell(new Phrase(str, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        }
                        else
                        {
                            oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.AttStatusInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        }
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZN.AttStatusInString, _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    }
                    _nTotalOT = _nTotalOT + oTempAttendanceDaily_ZN.OverTimeInMinute;
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

                    oPdfPCell = new PdfPCell(new Phrase("No Record Found", _oFontStyle)); oPdfPCell.FixedHeight = _nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                }
                _dEmployeeWorkingHour = _dEmployeeWorkingHour + oTempAttendanceDaily_ZN.TotalWorkingHourInMinute;
                oPdfPTable.CompleteRow();
                dstartDate = dstartDate.AddDays(1);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 4; oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Total Working Hr: " + Math.Floor(_dEmployeeWorkingHour / 60).ToString() + "h" + " " + Math.Floor(_dEmployeeWorkingHour % 60).ToString() + "m", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Total Shift Duration: " + Math.Floor(_dEmployeeShiftDurationInMin / 60).ToString() + "h" + " " + Math.Floor(_dEmployeeShiftDurationInMin % 60).ToString() + "m", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3; oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 10; oPdfPCell.FixedHeight = _nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        private TimeSpan GetTimeDiff(AttendanceDaily_ZN obj)
        {
            TimeSpan span = new TimeSpan();
            if (obj.ShiftStartTime < obj.ShiftEndTime)
            {
                span = obj.ShiftEndTime.Subtract(obj.ShiftStartTime);
            }
            else if (obj.ShiftStartTime > obj.ShiftEndTime)
            {
                int tMinuteDifference = (24 * 60) - ((obj.ShiftStartTime.Hour * 60) + obj.ShiftStartTime.Minute) + ((obj.ShiftEndTime.Hour * 60) + obj.ShiftEndTime.Minute);
                span = TimeSpan.FromMinutes(tMinuteDifference);
            }
            return span;
        }
        public PdfPTable GetFooterTable(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 15f, 50f, 50f, 50f, 40f, 40f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DAYS IN MONTH", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + _nCount.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF LEAVE", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            int nLeave = this.GetCountedDays(oAttendanceDaily_ZNs, "Leave");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nLeave.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF PRESENT", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            int nPresent = this.GetCountedDays(oAttendanceDaily_ZNs, "P");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nPresent.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF OFF DAY", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            int nDayOff = this.GetCountedDays(oAttendanceDaily_ZNs, "Off");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nDayOff.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF LATE", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            int nLate = this.GetCountedDays(oAttendanceDaily_ZNs, "Late");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nLate.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF HOLY DAY", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            int nHoliDay = this.GetCountedDays(oAttendanceDaily_ZNs, "HD");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nHoliDay.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF ABSENT", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            int nAbsent = this.GetCountedDays(oAttendanceDaily_ZNs, "A");
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nAbsent.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            string sWorkingSummaryLabel = (_dEmployeeShiftDurationInMin > _dEmployeeWorkingHour) ? "TOTAL LESS" : "TOTAL OVERTIME";

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase(sWorkingSummaryLabel, _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + Math.Floor(Math.Abs(_dEmployeeShiftDurationInMin - _dEmployeeWorkingHour) / 60) + "h" + " " + Math.Round(Math.Abs(_dEmployeeShiftDurationInMin - _dEmployeeWorkingHour) % 60, 0) + "m", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            _dEmployeeShiftDurationInMin = 0.00;
            _dEmployeeWorkingHour = 0.00;
            return oPdfPTable;
        }

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
    }

}
