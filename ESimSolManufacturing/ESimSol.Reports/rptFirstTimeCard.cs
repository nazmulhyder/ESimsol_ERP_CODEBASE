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
    public class rptFirstTimeCard
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
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
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs;
            _oCompany = oAttendanceDaily_ZN.Company;

            _dstartDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[0]);
            _dEndDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[1]);

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 240f, 20f, 240f});
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 75f, 120f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(200f, 35f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 30;
                oPdfPCellHearder.Colspan = 3;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = 2;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 35; oPdfPCellHearder.Colspan = 3;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }

            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 16;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 45;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase(" "));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 5;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("FIRST TIME CARD", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("(Black : Present with overtime, Green : Present, Blue : Off Day, Red : Absent, Light gray : Approve leave)", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            while (_oAttendanceDaily_ZNs.Count>0)
            {
                List<AttendanceDaily_ZN> oFirstADs = new List<AttendanceDaily_ZN>();
                oFirstADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).ToList();
                _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oFirstADs[0].EmployeeID);

                List<AttendanceDaily_ZN> oSecondADs = new List<AttendanceDaily_ZN>();
                oSecondADs = _oAttendanceDaily_ZNs.Where(x => x.EmployeeID == _oAttendanceDaily_ZNs[0].EmployeeID).ToList();
                _oAttendanceDaily_ZNs.RemoveAll(x => x.EmployeeID == oSecondADs[0].EmployeeID);
                EachRow(oFirstADs, oSecondADs);
            }
        }
        private void EachRow(List<AttendanceDaily_ZN> oFirstADs, List<AttendanceDaily_ZN> oSecondADs)
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (oFirstADs.Count > 0 && oSecondADs.Count > 0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oFirstADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (oFirstADs.Count > 0 && oSecondADs.Count > 0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oSecondADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();

            if (oFirstADs.Count > 0 && oSecondADs.Count<=0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oFirstADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            
                _oPdfPTable.CompleteRow();
            }
            if (oFirstADs.Count <= 0 && oSecondADs.Count >0)
            {
                _oPdfPCell = new PdfPCell(GetTimeCard(oSecondADs)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            
        }
        #endregion

        public PdfPTable GetTimeCard(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 15f, 50f, 50f, 50f,50f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPCell = new PdfPCell(new Phrase("Name :"+oAttendanceDaily_ZNs[0].EmployeeNameCode, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 5;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Designation :"+oAttendanceDaily_ZNs[0].DesignationName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 5;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Date Range :"+dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 5;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

          
            oPdfPTable.CompleteRow();



            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("SL.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Out", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("OT/NoWork", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            double nCount = 0;
            double nPresent = 0;
            double nAbsent = 0;
            double nLeave = 0;
            double nDayOff = 0;
            double nHoliDay = 0;

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
                        if (oTempAttendanceDaily_ZNs[i].InTimeInString == "-" && oTempAttendanceDaily_ZNs[i].OutTimeInString == "-")
                        {
                            if (oTempAttendanceDaily_ZNs[i].IsDayOff == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                                
                                oPdfPTable.CompleteRow();
                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsLeave == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.LIGHT_GRAY);
                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nLeave++;
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                           
                                oPdfPTable.CompleteRow();
                                nAbsent++;
                            }
                        }
                        else
                        {
                            if (oTempAttendanceDaily_ZNs[i].IsDayOff == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();

                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].OverTimeInMinute > 0)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();

                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.GREEN);

                                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OT_NowWork_FirstST, _oFontStyle)); oPdfPCell.FixedHeight = 20;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nPresent++;
                            }
                        }
                    }
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);

                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-".ToString(), _oFontStyle)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();

                }
                dstartDate = dstartDate.AddDays(1);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total Working Day : " + (nCount - nDayOff).ToString() + ", Present : " + nPresent.ToString() + ", Absent : " + nAbsent.ToString() + ", Leave : " + nLeave.ToString() + ", Off day : " + nDayOff + ", Holiday : " + nHoliDay, _oFontStyle));
            oPdfPCell.Colspan = 5; oPdfPCell.FixedHeight = 30;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            return oPdfPTable;

        }
    }




}
