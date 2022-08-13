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
    public class rptMamiyaTimeCard
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceDaily_ZN _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        List<AttendanceDaily_ZN> _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;

        #endregion

        public byte[] PrepareReport(AttendanceDaily_ZN oAttendanceDaily_ZN)
        {
            _oAttendanceDaily_ZN = oAttendanceDaily_ZN;
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
            _oCompany = oAttendanceDaily_ZN.Company;

            _dstartDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[0]);
            _dEndDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[1]);

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842, 595), 0f, 0f, 0f, 0f);//LANDSCAPE
            //_oDocument = new Document(PageSize.A4_LANDSCAPE, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE);
            _oDocument.SetMargins(5f, 5f, 15f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 200f, 5f, 200f, 5f, 200f, 5f, 200f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        //#region Report Header
        //private void PrintHeader()
        //{
        //    #region CompanyHeader

        //    #endregion

        //}
        //#endregion

        #region Report Body
        private void PrintBody()
        {
            if (_oAttendanceDaily_ZNs.Count > 0)
            {
                while (_oAttendanceDaily_ZNs.Count > 0)
                {
                    List<AttendanceDaily_ZN> oFirstADs = new List<AttendanceDaily_ZN>();
                    oFirstADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oFirstADs[0].EmployeeID);

                    List<AttendanceDaily_ZN> oSecondADs = new List<AttendanceDaily_ZN>();
                    oSecondADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oSecondADs[0].EmployeeID);

                    List<AttendanceDaily_ZN> oThirdADs = new List<AttendanceDaily_ZN>();
                    oThirdADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oThirdADs[0].EmployeeID);

                    List<AttendanceDaily_ZN> oFourthADs = new List<AttendanceDaily_ZN>();
                    oFourthADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oFourthADs[0].EmployeeID);

                    EachRow(oFirstADs, oSecondADs, oThirdADs, oFourthADs);
                }
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to Print!!", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        private void EachRow(List<AttendanceDaily_ZN> oFirstADs, List<AttendanceDaily_ZN> oSecondADs, List<AttendanceDaily_ZN> oThirdADs, List<AttendanceDaily_ZN> oFourthADs)
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

        public PdfPTable GetTimeCard(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            double OT_H = 0;
            double OT_NH = 0;
            double OT_Total = 0;
            double CL_Full = 0;
            double CL_Half = 0;
            double CL_Short = 0;
            double SL = 0;
            double EL_Full = 0;
            double EL_Half = 0;
            double EL_Short = 0;
            double ML_Full = 0;
            double ML_Half = 0;
            double ML_Short = 0;
            double LWP = 0;
            double RL = 0;

            int nLateArrival = 0;
            int nEarlyLeave = 0;
            if (oAttendanceDaily_ZNs.Count > 0)
            {
                nLateArrival = oAttendanceDaily_ZNs.Where(x => x.LateArrivalMinute > 0).ToList().Count();
            }
            if (oAttendanceDaily_ZNs.Count > 0)
            {
                nEarlyLeave = oAttendanceDaily_ZNs.Where(x => x.EarlyDepartureMinute > 0).ToList().Count();
            }

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

            //if (_oCompany.CompanyLogo != null)
            //{
            //    _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    _oImag.ScaleAbsolute(40f, 25f);
            //    oPdfPCellHearder = new PdfPCell(_oImag);
            //    oPdfPCellHearder.FixedHeight = 25;
            //    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
            //    oPdfPCellHearder.PaddingBottom = 2;
            //    oPdfPCellHearder.Border = 0;

            //    oPdfPTableHeader.AddCell(oPdfPCellHearder);
            //}
            //else
            //{
            //    oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
            //    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            //}

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

            oPdfPCell = new PdfPCell(new Phrase("Name :" + oAttendanceDaily_ZNs[0].EmployeeNameCode, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Designation :" + oAttendanceDaily_ZNs[0].DesignationName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Joining :" + oAttendanceDaily_ZNs[0].JoiningDateInString, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6;
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
           
            double nPresent = 0;
            double nAbsent = 0;
            double nLeave = 0;
            double nDayOff = 0;
            double nHoliDay = 0;
            int nCount = 0;

            dEndDate = dEndDate.AddDays(1);
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                nCount++;
                List<AttendanceDaily_ZN> oTempAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
                oTempAttendanceDaily_ZNs = oAttendanceDaily_ZNs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList();

                if (oTempAttendanceDaily_ZNs.Count > 0)
                {
                    for (int i = 0; i < oTempAttendanceDaily_ZNs.Count; i = i + 1)
                    {
                        string Status = "";
                        if (oTempAttendanceDaily_ZNs[i].InTimeInString == "-" && oTempAttendanceDaily_ZNs[i].OutTimeInString == "-" && oTempAttendanceDaily_ZNs[i].IsOSD == false)
                        {
                            if (oTempAttendanceDaily_ZNs[i].IsDayOff == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        
                                 oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                 oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "Off";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                Status = "HD";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsLeave == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                                
                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                                
                                oPdfPTable.CompleteRow();
                                nLeave++;
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "A";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
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

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "Off";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();

                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "HD";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();

                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].OverTimeInMinute > 0)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "P";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full){ nLeave++; }else { nPresent++; }
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                               
                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); 
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "P";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].ShiftName, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();

                                if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full){ nLeave++; }else { nPresent++;}
                            }
                        }

                        if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "CL")
                        {
                            if (oTempAttendanceDaily_ZNs[i].LeaveType==EnumLeaveType.Full)
                            {
                                CL_Full++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                            {
                                CL_Half++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                            {
                                CL_Short++;
                            }
                        }
                        else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "EL")
                        {
                            if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                            {
                                EL_Full++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                            {
                                EL_Half++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                            {
                                EL_Short++;
                            }
                        }
                        else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "ML")
                        {
                            if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                            {
                                ML_Full++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                            {
                                ML_Half++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                            {
                                ML_Short++;
                            }
                        }
                        else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "SL")
                        {
                            if (oTempAttendanceDaily_ZNs[i].LeaveType==EnumLeaveType.Full)
                            {
                                SL += 8;// Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration);
                            }
                            else if(oTempAttendanceDaily_ZNs[i].LeaveType==EnumLeaveType.Half)
                            {
                                SL += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration)/60;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                            {
                                //TimeSpan ts = oTempAttendanceDaily_ZNs[i].OutTime-oTempAttendanceDaily_ZNs[i].InTime;
                                //double nDuration= ts.Hours;
                                //SL+= 8 - nDuration;
                                SL += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                            }
                        }
                        else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "LWP")
                        {
                            if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                            {
                                LWP += 8;// Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration);
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                            {
                                //LWP+= 4;
                                LWP += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                            {
                                //TimeSpan ts = oTempAttendanceDaily_ZNs[i].OutTime - oTempAttendanceDaily_ZNs[i].InTime;
                                //double nDuration = ts.Hours;
                                //LWP+= 8 - nDuration;
                                LWP += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                            }
                        }
                        else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "RL")
                        {
                            if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                            {
                                RL++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                            {
                                RL++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                            {
                                RL++;
                            }
                        }

                        OT_H+=oTempAttendanceDaily_ZNs[i].OT_HHR;
                        OT_NH+=oTempAttendanceDaily_ZNs[i].OT_NHR;
                        OT_Total+=oTempAttendanceDaily_ZNs[i].OverTimeInMinute;
                    }
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                    oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); 
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); 
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-".ToString(), _oFontStyle)); 
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("".ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    //nAbsent++;
                }
                dstartDate = dstartDate.AddDays(1);


            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase("WD-" + (nCount - nDayOff - nHoliDay).ToString() + ", P-" + nPresent.ToString() + ", A-" + nAbsent.ToString() + ", L-" + nLeave.ToString() + ", Off-" + nDayOff + ", H-" + nHoliDay + ", LT-" + nLateArrival + ", E-" + nEarlyLeave, _oFontStyle));
            oPdfPCell.Colspan = 6; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            string sLeaveStatus = "";

            if (CL_Full > 0 || CL_Half > 0 || CL_Short > 0)
            {
                sLeaveStatus += "CL(";
                if (CL_Full > 0)
                {
                    sLeaveStatus += "F-" + CL_Full.ToString() + ",";
                }
                if (CL_Half > 0)
                {
                    sLeaveStatus += "H-" + CL_Half.ToString() + ",";
                }
                if (CL_Short > 0)
                {
                    sLeaveStatus += "S-" + CL_Short.ToString() + ",";
                }
                sLeaveStatus = sLeaveStatus.Remove(sLeaveStatus.Length - 1, 1);
                sLeaveStatus += ")";
            }

            if (EL_Full > 0 || EL_Half > 0 || EL_Short > 0)
            {
                sLeaveStatus += ",";
                sLeaveStatus += "EL(";                
                if (EL_Full > 0)
                {
                    sLeaveStatus += "F-" + EL_Full.ToString() + ",";
                }
                if (EL_Half > 0)
                {
                    sLeaveStatus += "H-" + EL_Half.ToString() + ",";
                }
                if (EL_Short > 0)
                {
                    sLeaveStatus += "S-" + EL_Short.ToString() + ",";
                }
                sLeaveStatus = sLeaveStatus.Remove(sLeaveStatus.Length - 1, 1);
                sLeaveStatus += ")";
            }
            if (SL > 0)
            {
                sLeaveStatus += ",SL-" + Global.MillionFormat(SL)+"hr";
            }
            if (LWP > 0)
            {
                sLeaveStatus += ",LWP-" + Global.MillionFormat(LWP) + "hr";
            }

            if (ML_Full > 0 || ML_Half > 0 || ML_Short > 0)
            {
                sLeaveStatus += ",";
                sLeaveStatus += "ML(";                
                if (ML_Full > 0)
                {
                    sLeaveStatus += "F-" + ML_Full.ToString() + ",";
                }
                if (ML_Half > 0)
                {
                    sLeaveStatus += "H-" + ML_Half.ToString();
                }
                if (ML_Short > 0)
                {
                    sLeaveStatus += "S-" + EL_Short.ToString() + ",";
                }
                sLeaveStatus = sLeaveStatus.Remove(sLeaveStatus.Length - 1, 1);
                sLeaveStatus += ")";
            }
            if (RL > 0)
            {
                sLeaveStatus += ",RL-" + RL + " d";
            }
            if (OT_H > 0)
            {

                sLeaveStatus += ",OTHH-" + Global.MinInHourMin(Convert.ToInt32(OT_H));
            }
            if (OT_NH > 0)
            {
                sLeaveStatus += ",OTNH-" + Global.MinInHourMin(Convert.ToInt32(OT_NH));
            }

            if (OT_Total > 0)
            {
                sLeaveStatus += ",TOT-" + Global.MinInHourMin(Convert.ToInt32(OT_Total));
            }
            if (sLeaveStatus!="" && sLeaveStatus[0] == ',')
            {
                sLeaveStatus = sLeaveStatus.Remove(0, 1);
            }

            oPdfPCell = new PdfPCell(new Phrase(sLeaveStatus, _oFontStyle));
            oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            return oPdfPTable;

        }
    }




}
