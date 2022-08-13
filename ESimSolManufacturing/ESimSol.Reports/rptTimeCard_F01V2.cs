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
    public class rptTimeCard_F01V2
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
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
            _oDocument = new Document(new iTextSharp.text.Rectangle(842, 595), 0f, 0f, 0f, 0f);//LANDSCAPE
            _oDocument.SetMargins(5f, 5f, 15f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 200f, 5f, 200f, 5f, 200f, 5f, 200f });
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
                    List<AttendanceDailyV2> oFirstADs = new List<AttendanceDailyV2>();
                    oFirstADs = _oAttendanceDailyV2s.Where(x => x.EmployeeID == _oAttendanceDailyV2s[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oFirstADs[0].EmployeeID);

                    List<AttendanceDailyV2> oSecondADs = new List<AttendanceDailyV2>();
                    oSecondADs = _oAttendanceDailyV2s.Where(x => x.EmployeeID == _oAttendanceDailyV2s[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oSecondADs[0].EmployeeID);

                    List<AttendanceDailyV2> oThirdADs = new List<AttendanceDailyV2>();
                    oThirdADs = _oAttendanceDailyV2s.Where(x => x.EmployeeID == _oAttendanceDailyV2s[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oThirdADs[0].EmployeeID);

                    List<AttendanceDailyV2> oFourthADs = new List<AttendanceDailyV2>();
                    oFourthADs = _oAttendanceDailyV2s.Where(x => x.EmployeeID == _oAttendanceDailyV2s[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oFourthADs[0].EmployeeID);

                    EachRow(oFirstADs, oSecondADs, oThirdADs, oFourthADs);
                }
            }

        }
        private void EachRow(List<AttendanceDailyV2> oFirstADs, List<AttendanceDailyV2> oSecondADs, List<AttendanceDailyV2> oThirdADs, List<AttendanceDailyV2> oFourthADs)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            bool IsFirst = false;
            bool IsSecond = false;
            bool IsThird = false;
            bool IsFourth = false;

            if (oFirstADs.Count > 0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oFirstADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                IsFirst = true;
            }

            if (oSecondADs.Count > 0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oSecondADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                IsSecond = true;
            }

            if (oThirdADs.Count > 0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oThirdADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                IsThird = true;
            }

            if (oFourthADs.Count > 0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oFourthADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                IsFourth = true;
            }

            if (IsFirst)
            {
                if (!IsSecond)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                if (!IsThird)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                if (!IsFourth)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
            }

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion

        public PdfPTable GetTimeCard(List<AttendanceDailyV2> oAttendanceDailyV2s)
        {
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 55f, 28f, 28f, 33f, 32f, 25f });

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 70f, 125f });
            PdfPCell oPdfPCellHearder;



            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
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


            oPdfPCell = new PdfPCell(oPdfPTableHeader); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("TIME CARD", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();



            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase("Name :" + oAttendanceDailyV2s[0].EmployeeName+"["+oAttendanceDailyV2s[0].EmployeeCode+"]", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Designation :" + oAttendanceDailyV2s[0].DesignationName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Joining :" + oAttendanceDailyV2s[0].JoiningDateInString, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Date Range :" + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Out", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("OT/NW", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shift", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0, nExactWD = 0 ;

            dEndDate = dEndDate.AddDays(1);
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                nCount++;
                AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                oTempAttendanceDailyV2 = oAttendanceDailyV2s.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                if (oTempAttendanceDailyV2 != null)
                {
                    nExactWD++;
                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.InTimeInString, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.OutTimeInString, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDailyV2.OverTimeInMin), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.AttStatusInString, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDailyV2.ShiftName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-".ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("No record Found".ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                dstartDate = dstartDate.AddDays(1);
            }

            int nTotalDayOff = oAttendanceDailyV2s.Where(x => x.AttStatusInString == "Off").ToList().Count();
            int nTotalHoliday = oAttendanceDailyV2s.Where(x => x.AttStatusInString == "HD").ToList().Count();
            int nTWD = nExactWD - (nTotalDayOff + nTotalHoliday);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase("WD-" + nTWD.ToString() + ", P-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "P", false) + ", A-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "A", false) + ", L-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "Leave", false) + ", Off-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "Off", false) + ", H-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "HD", false) + ", LT-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "Late", false) + ", E-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "Early Out Days", false), _oFontStyle));
            oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("OTNH-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "OT") + ", TOT-" + AttendanceDailyV2.GetAttendanceSummary(oAttendanceDailyV2s, "OT"), _oFontStyle));
            oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            return oPdfPTable;

        }
    }
}
