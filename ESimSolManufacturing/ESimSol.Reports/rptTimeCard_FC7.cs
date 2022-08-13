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
    public class rptTimeCard_FC7
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
        List<MaxOTConfiguration> _oMaxOTConfiguration = new List<MaxOTConfiguration>();
        Company _oCompany = new Company();
        DateTime _dstartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;

        #endregion

        public byte[] PrepareReport(AttendanceDaily_ZN oAttendanceDaily_ZN)
        {
            _oAttendanceDaily_ZN = oAttendanceDaily_ZN;
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs;
            _oCompany = oAttendanceDaily_ZN.Company;
            _oMaxOTConfiguration = oAttendanceDaily_ZN.MaxOTConfiguration;

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

                _oPdfPCell = new PdfPCell(GetFooterTable()); _oPdfPCell.Colspan = 4;
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

        double nPresent = 0;
        double nAbsent = 0;
        double nLeave = 0;
        double nDayOff = 0;
        double nHoliDay = 0;
        double nLate = 0;
        int nTotalOT = 0;
        int nCount = 0;
        int nFixedHeight = 0; 
        int MaxOTINMIN = 0;
        int showActualMaxOTINMIN = 0;
        public PdfPTable GetTimeCard(List<AttendanceDaily_ZN> oAttendanceDaily_ZNs)
        {
            //double OT_H = 0;
            //double OT_NH = 0;
            //double OT_Total = 0;
            //double CL_Full = 0;
            //double CL_Half = 0;
            //double CL_Short = 0;
            //double SL = 0;
            //double EL_Full = 0;
            //double EL_Half = 0;
            //double EL_Short = 0;
            //double ML_Full = 0;
            //double ML_Half = 0;
            //double ML_Short = 0;
            //double LWP = 0;

            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;

            dstartDate = _dstartDate;
            dEndDate = _dEndDate;

            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 15f, 50f, 70f, 70f, 40f, 40f });

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 80f, 120f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(100f, 25f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                //oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = -5;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
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

            oPdfPCell = new PdfPCell(oPdfPTableHeader);
            oPdfPCell.Colspan = 6;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" "));
            oPdfPCell.Colspan = 6;
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 7;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("TIME CARD", _oFontStyle));
            oPdfPCell.Colspan = 6;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("" , _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("UNIT : " +(oAttendanceDaily_ZNs.Count>0?oAttendanceDaily_ZNs[0].LocationName:""), _oFontStyle));
            oPdfPCell.Colspan = 2;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase("Print Date : " + DateTime.Now.ToString("dd MMM yyyy HH:mm"), _oFontStyle));
            oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; 
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6; oPdfPCell.FixedHeight = 10;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            nFixedHeight = 15;

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("CODE", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].EmployeeCode, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].EmployeeName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell = new PdfPCell(new Phrase("DEPARTMENT", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].DepartmentName, _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

           
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("DESIGNATION", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].DesignationName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("JOINING", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell = new PdfPCell(new Phrase(": " + oAttendanceDaily_ZNs[0].JoiningDateInString, _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

           
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("DATE RANGE", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


           nFixedHeight=16;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("In", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Out", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("OT HR", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Status", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

             nPresent = 0;
             nAbsent = 0;
             nLeave = 0;
             nDayOff = 0;
             nHoliDay = 0;
             nCount = 0;
             nLate = 0;
             nTotalOT = 0;

            dEndDate = dEndDate.AddDays(1);
            while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                nCount++;
                List<AttendanceDaily_ZN> oTempAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
                oTempAttendanceDaily_ZNs = oAttendanceDaily_ZNs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList();

                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                if (oTempAttendanceDaily_ZNs.Count > 0)
                {
                    for (int i = 0; i < oTempAttendanceDaily_ZNs.Count; i = i + 1)
                    {
                        foreach (MaxOTConfiguration oItem in _oMaxOTConfiguration)
                        {
                            MaxOTINMIN = oItem.MaxOTInMin;
                        }
                        if (oTempAttendanceDaily_ZNs[i].OverTimeInMinute > MaxOTINMIN)
                        {
                            showActualMaxOTINMIN = MaxOTINMIN;
                        }
                        else
                        {
                            showActualMaxOTINMIN = oTempAttendanceDaily_ZNs[i].OverTimeInMinute;
                        }

                        string Status = "";
                        if (oTempAttendanceDaily_ZNs[i].InTimeInString == "-" && oTempAttendanceDaily_ZNs[i].OutTimeInString == "-" && oTempAttendanceDaily_ZNs[i].IsOSD == false)
                        {
                            
                            if (oTempAttendanceDaily_ZNs[i].IsDayOff == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //Overtime calculation according to MaxOTConfiguration
                                
                                
                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                //oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oTempAttendanceDaily_ZNs[i].OverTimeInMinute), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "Off";

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle));
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "HD";

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsLeave == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nLeave++;
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "A";

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
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

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "Off";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                if (oTempAttendanceDaily_ZNs[i].LateArrivalMinute > 0)
                                {
                                    Status += ",Late";
                                    nLate++;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nDayOff++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsHoliday == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "HD";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += "," + oTempAttendanceDaily_ZNs[i].LeaveName;
                                }
                                if (oTempAttendanceDaily_ZNs[i].LateArrivalMinute > 0)
                                {
                                    Status += ",Late";
                                    nLate++;
                                }
                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                               
                                oPdfPTable.CompleteRow();
                                nHoliDay++;
                            }
                            else if (oTempAttendanceDaily_ZNs[i].IsLeave == true)
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }

                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nLeave++;
                            }
                            else
                            {
                                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                                oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].InTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(oTempAttendanceDaily_ZNs[i].OutTimeInString, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(showActualMaxOTINMIN), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                Status = "P";

                                if (oTempAttendanceDaily_ZNs[i].LeaveName != "")
                                {
                                    Status += oTempAttendanceDaily_ZNs[i].LeaveName;
                                }
                                if (oTempAttendanceDaily_ZNs[i].LateArrivalMinute > 0)
                                {
                                    Status += ",Late";
                                    nLate++;
                                }
                                oPdfPCell = new PdfPCell(new Phrase(Status, _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                                oPdfPTable.CompleteRow();
                                nPresent++;
                            }
                        }


                        //if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "CL")
                        //{
                        //    if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                        //    {
                        //        CL_Full++;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                        //    {
                        //        CL_Half++;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                        //    {
                        //        CL_Short++;
                        //    }
                        //}
                        //else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "EL")
                        //{
                        //    if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                        //    {
                        //        EL_Full++;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                        //    {
                        //        EL_Half++;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                        //    {
                        //        EL_Short++;
                        //    }
                        //}
                        //else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "ML")
                        //{
                        //    if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                        //    {
                        //        ML_Full++;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                        //    {
                        //        ML_Half++;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                        //    {
                        //        ML_Short++;
                        //    }
                        //}
                        //else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "SL")
                        //{
                        //    if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                        //    {
                        //        SL += 8;// Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration);
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                        //    {
                        //        SL += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                        //    {
                        //        //TimeSpan ts = oTempAttendanceDaily_ZNs[i].OutTime-oTempAttendanceDaily_ZNs[i].InTime;
                        //        //double nDuration= ts.Hours;
                        //        //SL+= 8 - nDuration;
                        //        SL += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                        //    }
                        //}
                        //else if (oTempAttendanceDaily_ZNs[i].LeaveName != "" && oTempAttendanceDaily_ZNs[i].LeaveName == "LWP")
                        //{
                        //    if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Full)
                        //    {
                        //        LWP += 8;// Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration);
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Half)
                        //    {
                        //        //LWP+= 4;
                        //        LWP += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                        //    }
                        //    else if (oTempAttendanceDaily_ZNs[i].LeaveType == EnumLeaveType.Short)
                        //    {
                        //        //TimeSpan ts = oTempAttendanceDaily_ZNs[i].OutTime - oTempAttendanceDaily_ZNs[i].InTime;
                        //        //double nDuration = ts.Hours;
                        //        //LWP+= 8 - nDuration;
                        //        LWP += Convert.ToDouble(oTempAttendanceDaily_ZNs[i].LeaveDuration) / 60;
                        //    }
                        //}

                        //OT_H += oTempAttendanceDaily_ZNs[i].OT_HHR;
                        //OT_NH += oTempAttendanceDaily_ZNs[i].OT_NHR;
                        //OT_Total += oTempAttendanceDaily_ZNs[i].OverTimeInMinute;
                        if (oTempAttendanceDaily_ZNs[i].OverTimeInMinute > 0)
                        {
                            nTotalOT += showActualMaxOTINMIN;
                            //nTotalOT += oTempAttendanceDaily_ZNs[i].OverTimeInMinute;
                        }
                    }
                }
                else
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                    oPdfPCell = new PdfPCell(new Phrase(dstartDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = nFixedHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    //nAbsent++;
                }
                dstartDate = dstartDate.AddDays(1);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 6; oPdfPCell.FixedHeight = nFixedHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            //oPdfPCell = new PdfPCell(new Phrase("WD-" + (nCount - nDayOff - nHoliDay).ToString() + ", P-" + nPresent.ToString() + ", A-" + nAbsent.ToString() + ", L-" + nLeave.ToString() + ", Off-" + nDayOff + ", H-" + nHoliDay, _oFontStyle));
            //oPdfPCell.Colspan = 6;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //oPdfPTable.CompleteRow();

            
            return oPdfPTable;

        }

        public PdfPTable GetFooterTable()
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
            oPdfPCell = new PdfPCell(new Phrase(": "+ nCount.ToString()+" DAYS", _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

           
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF LEAVE" , _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nLeave.ToString()+" DAYS", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF PRESENT", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": "+ nPresent.ToString()+" DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF OFF DAY", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nDayOff.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF LATE", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nLate.ToString() + " DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF HOLI DAY", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + nHoliDay.ToString()+" DAYS", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NUMBER OF ABSENT", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": "+ nAbsent.ToString()+" DAYS", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("TOTAL OVERTIME", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(": " + Global.MinInHourMin(nTotalOT), _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }

    }

}

