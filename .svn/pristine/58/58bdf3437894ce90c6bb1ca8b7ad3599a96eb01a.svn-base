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

namespace ESimSol.Reports
{

    public class rptDateWiseAttendance_Summery
    {
        #region Declaration
        int _nColumns = 0;
        int _nPageWidth = 0;
        int _npageHeight = 550;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceDaily _oAttendanceDaily = new AttendanceDaily();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        Company _oCompany = new Company();
        DateTime _sStartDate = DateTime.Now;
        DateTime _sEndDate = DateTime.Now;
 
        int _ColSpan1 = 0;
        int _nMonthCycle = 0;

        #endregion

        public byte[] PrepareReport(AttendanceDaily oAttendanceDaily, List<BusinessUnit> oBusinessUnits)
        {
            _oAttendanceDailys = oAttendanceDaily.AttendanceDailys.OrderBy(x => x.EmployeeCode).ToList();
            _oCompany = oAttendanceDaily.Company;
            _oBusinessUnits = oBusinessUnits;
            _sStartDate = Convert.ToDateTime(oAttendanceDaily.ErrorMessage.Split(',')[0]);
            _sEndDate = Convert.ToDateTime(oAttendanceDaily.ErrorMessage.Split(',')[1]);

            if (_sStartDate.Month == _sEndDate.Month)
            {
                _ColSpan1 = (_sEndDate.Day - _sStartDate.Day+1)*2;
            }
            else
            {
                _ColSpan1 = (DateTime.DaysInMonth(_sStartDate.Year, _sStartDate.Month) - Convert.ToInt32(_sStartDate.Day)+1)*2;
            }

            #region Page Setup

            TimeSpan diff = _sEndDate - _sStartDate;
            _nMonthCycle = diff.Days + 1;
            _nColumns = _nMonthCycle * 2 + 4;

            //float[] tablecolumns = new float[_nColumns + 9];
            float[] tablecolumns = new float[_nColumns + 9];
            _nPageWidth = 900;
            tablecolumns[0] = 10f;
            tablecolumns[1] = 15f;
            tablecolumns[2] = 15f;
            tablecolumns[3] = 15f;
        
            for (int i = 4; i < _nColumns; i++)
            {
                tablecolumns[i] = 10f;
            }

            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 25f;
            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 15f;
            tablecolumns[_nColumns++] = 15f;
            //tablecolumns[_nColumns++] = 15f;



            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 9;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            if (_oBusinessUnits.Count > 1 || _oBusinessUnits.Count == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnits[0].Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            if (_oBusinessUnits.Count > 1 || _oBusinessUnits.Count == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnits[0].Address, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Attendance Summery", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate.ToString("dd MMM yyyy") + " To " + _sEndDate.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Card No", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_sStartDate.ToString("MMM"), _oFontStyle)); _oPdfPCell.Colspan = _ColSpan1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (_sStartDate.Month != _sEndDate.Month)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_sEndDate.ToString("MMM"), _oFontStyle)); _oPdfPCell.Colspan = _sEndDate.Day*2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
           
            _oPdfPCell = new PdfPCell(new Phrase("PD", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Absent", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Off", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("HD", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LD", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Half/Short L", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TD", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Joining", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Date Of Birth", _oFontStyle)); _oPdfPCell.Rowspan = 3; _oPdfPCell.Rotation = 90;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            DateTime dStartDate = _sStartDate;
            while (dStartDate <= _sEndDate)
            {
                _oPdfPCell = new PdfPCell(new Phrase(dStartDate.Day.ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                dStartDate = dStartDate.AddDays(1);

            }

            _oPdfPTable.CompleteRow();

            dStartDate = _sStartDate;
            while (dStartDate <= _sEndDate)
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Out", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("OT", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                
                
                dStartDate = dStartDate.AddDays(1);

            }

            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            while (_oAttendanceDailys.Count > 0)
            {
                List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
                oAttendanceDailys = _oAttendanceDailys.Where(x => x.EmployeeID == _oAttendanceDailys[0].EmployeeID).ToList();
                BodyMake(oAttendanceDailys.OrderBy(x => x.AttendanceDate).ToList());
                _oAttendanceDailys.RemoveAll(x => x.EmployeeID == oAttendanceDailys[0].EmployeeID);

            }

            Footer();

        }
        int nCount = 0;
        int nWorkingDay = 0;
        int nHDay = 0;
        int nDayOff = 0;
        int nLDay = 0;
        int nSHLDay = 0;
        int nTWDay = 0;
        int nOTH = 0;
        int nOTM = 0;
        int nAbsent = 0;

        public void BodyMake(List<AttendanceDaily> oADs)
        {

            nWorkingDay = 0;
            nHDay = 0;
            nLDay = 0;
            nTWDay = 0;
            nOTH = 0;
            nOTM = 0;
            nAbsent = 0;
            nDayOff = 0;
            if (oADs.Count > 0)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADs[0].EmployeeCode, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADs[0].EmployeeName, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADs[0].DesignationName, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                DateTime dStartDate = _sStartDate;
                while (dStartDate <= _sEndDate)
                {
                    bool bFlag = false;

                    foreach (AttendanceDaily oADItem in oADs)
                    {

                        if (dStartDate == oADItem.AttendanceDate)
                        {
                            bFlag = true;
                            AttendanceExist(oADItem);
                            break;
                        }

                    }
                    if (!bFlag)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("N/A", _oFontStyle)); _oPdfPCell.Rotation = 90;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Rotation = 90;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nAbsent++;
                    }
                    dStartDate = dStartDate.AddDays(1);
                }
            }

            //if (_nColumns - 11 > oADs.Count)
            //{
            //    for (int j = 0; j < (_nColumns - oADs.Count - 11); j++)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    }
            //}

            _oPdfPCell = new PdfPCell(new Phrase(nWorkingDay.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nAbsent.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nDayOff.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nHDay.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nLDay.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nSHLDay.ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((nWorkingDay + nDayOff + nHDay + nLDay).ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_nMonthCycle).ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(((nOTM / 60).ToString()).Split('.')[0] + ":" + (nOTM % 60).ToString(), _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(oADs[0].JoiningDateInString, _oFontStyle)); _oPdfPCell.Rotation = 90;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(oADs[0].DateOfBirthInString, _oFontStyle)); _oPdfPCell.Rotation = 90;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            if (nCount % 32 == 0)
            {
                Footer();
            }

        }
        public void AttendanceExist(AttendanceDaily oADItem)
        {
            if ((oADItem.InTimeInString != "-" || oADItem.OutTimeInString != "-" || oADItem.IsOSD== true) && oADItem.IsDayOff == false && oADItem.IsLeave == false && oADItem.IsNoWork == false && oADItem.IsHoliday == false)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oADItem.InTimeInString + "-" + oADItem.OutTimeInString, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nWorkingDay++;
            }
            else if ((oADItem.InTimeInString != "-" || oADItem.OutTimeInString != "-" || oADItem.IsOSD== true) && oADItem.IsDayOff == false && oADItem.IsLeave == false && oADItem.IsNoWork == true && oADItem.IsHoliday == false)
            {
                _oPdfPCell = new PdfPCell(new Phrase("NW(" + oADItem.InTimeInString + "-" + oADItem.OutTimeInString+")", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nWorkingDay++;
            }
            else if ((oADItem.InTimeInString != "-" || oADItem.OutTimeInString != "-" || oADItem.IsOSD== true) && oADItem.IsDayOff == true && oADItem.IsLeave == false && oADItem.IsNoWork == false && oADItem.IsHoliday == false)
            {
                _oPdfPCell = new PdfPCell(new Phrase("DO(" + oADItem.InTimeInString + "-" + oADItem.OutTimeInString + ")", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; 
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nDayOff++;
            }
            else if ((oADItem.InTimeInString != "-" || oADItem.OutTimeInString != "-" || oADItem.IsOSD== true) && oADItem.IsDayOff == false && oADItem.IsLeave == false && oADItem.IsNoWork == false && oADItem.IsHoliday == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("HD(" + oADItem.InTimeInString + "-" + oADItem.OutTimeInString + ")", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                nHDay++;
            }
            else if (oADItem.InTimeInString == "-" && oADItem.OutTimeInString == "-" && oADItem.IsOSD== false && oADItem.IsDayOff == false && oADItem.IsLeave == false && oADItem.IsNoWork == false && oADItem.IsHoliday == false)
            {
                _oPdfPCell = new PdfPCell(new Phrase("A", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nAbsent++;
            }
            else if (oADItem.IsDayOff == true && oADItem.InTimeInString == "-" && oADItem.OutTimeInString == "-")
            {
                _oPdfPCell = new PdfPCell(new Phrase("DO", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nDayOff++;
            }
            else if (oADItem.IsLeave == true && oADItem.LeaveType == EnumLeaveType.Full)
            {
                _oPdfPCell = new PdfPCell(new Phrase("L", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nLDay++;
            }
            else if (oADItem.IsLeave == true && oADItem.LeaveType == EnumLeaveType.Short || oADItem.LeaveType == EnumLeaveType.Half)
            {
                _oPdfPCell = new PdfPCell(new Phrase("S/H L", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nSHLDay++;
                nWorkingDay++;
            }
            else if (oADItem.IsHoliday == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase("HD", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nHDay++;
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("N/A", _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oADItem.OTHrSt, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }
            nOTM += oADItem.OverTimeInMinute;
        }
        public void Footer()
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 50;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            int nColSpan1 = 0;
            int nColSpan2 = 0;
            int nColSpan3 = 0;

            if (_nColumns % 3 == 0)
            {
                nColSpan1 = _nColumns / 3;
                nColSpan2 = _nColumns / 3;
                nColSpan3 = _nColumns / 3;
            }
            else
            {
                nColSpan1 = (_nColumns - _nColumns % 3) / 3;
                nColSpan2 = (_nColumns - _nColumns % 3) / 3 + _nColumns % 3;
                nColSpan3 = (_nColumns - _nColumns % 3) / 3;
            }

            _oPdfPCell = new PdfPCell(new Phrase("_________________________\nPrepared By", _oFontStyle)); _oPdfPCell.Colspan = nColSpan1; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_________________________\nChecked By APM/PM", _oFontStyle)); _oPdfPCell.Colspan = nColSpan2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("_________________________\nApproved By", _oFontStyle)); _oPdfPCell.Colspan = nColSpan3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        #endregion
    }
}
