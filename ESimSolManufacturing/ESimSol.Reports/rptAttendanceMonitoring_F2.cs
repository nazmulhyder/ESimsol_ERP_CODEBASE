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
    public class rptAttendanceMonitoring_F2
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Image _oImag;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceMonitoring _oAttendanceMonitoring = new AttendanceMonitoring();
        List<AttendanceMonitoring> _oAttendanceMonitorings = new List<AttendanceMonitoring>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        Company _oCompany = new Company();
        string _sDate = "";

        #endregion

        public byte[] PrepareReport(AttendanceMonitoring oAttendanceMonitoring, DateTime dDate, List<BusinessUnit> oBusinessUnits)
        {
            _oAttendanceMonitorings = oAttendanceMonitoring.AttendanceMonitorings;
            _oCompany = oAttendanceMonitoring.Company;
            _oBusinessUnits = oBusinessUnits;
            _sDate = dDate.ToString("dd MMM yyyy");
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 25f, 120f, 80f, 80f, 80f, 80f, 80f });

            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(3);
            oPdfPTableHeader.SetWidths(new float[] { 210f, 150f, 140f });
            PdfPCell oPdfPCellHearder;

            if (_oBusinessUnits.Count > 1 || _oBusinessUnits.Count == 0)
            {
                if (_oCompany.CompanyLogo != null)
                {
                    _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _oImag.ScaleAbsolute(20f, 15f);
                    oPdfPCellHearder = new PdfPCell(_oImag);
                    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                    oPdfPCellHearder.PaddingBottom = 2;
                    oPdfPCellHearder.Border = 0;
                    oPdfPCellHearder.FixedHeight = 15;
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
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPCellHearder.Colspan = 2;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
                oPdfPCellHearder.Colspan = 3;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTableHeader);
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits[0].Name, _oFontStyle));
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPCellHearder.Colspan = 2;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits[0].Address, _oFontStyle));
                oPdfPCellHearder.Colspan = 3;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder.ExtraParagraphSpace = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
                oPdfPTableHeader.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTableHeader);
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("DAILY ATTENDANCE REPORT"));
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date-" +_sDate, _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell); _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            int nCount = 0;
            double nTotalActive = 0;
            double nTotalPresent = 0;
            double nTotalPresentPercent = 0;
            double nTotalAbsent = 0;
            double nTotalAbsentPercent = 0;

            double nGrandTotalActive = 0;
            double nGrandTotalPresent = 0;
            double nGrandTotalPresentPercent = 0;
            double nGrandTotalAbsent = 0;
            double nGrandTotalAbsentPercent = 0;

            while (_oAttendanceMonitorings.Count > 0)
            {
                List<AttendanceMonitoring> oTempAttendanceMonitorings = new List<AttendanceMonitoring>();
                oTempAttendanceMonitorings = _oAttendanceMonitorings.Where(x => x.BUName == _oAttendanceMonitorings[0].BUName).ToList();

                _oPdfPCell = new PdfPCell(new Phrase("Business Unit-" + _oAttendanceMonitorings[0].BUName, _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell); _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("DEPARTMETN", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("ACTIVE", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("ABSENT", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("ABSENT(%)", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PRESENT", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PRESENT(%)", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                while (oTempAttendanceMonitorings.Count > 0)
                {
                    List<AttendanceMonitoring> oTempAMs = new List<AttendanceMonitoring>();
                    oTempAMs = oTempAttendanceMonitorings.Where(x => x.DepartmentName == oTempAttendanceMonitorings[0].DepartmentName).ToList();

                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempAMs[0].DepartmentName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    double nActive = oTempAMs.Sum(x => x.Exists);
                    _oPdfPCell = new PdfPCell(new Phrase(nActive > 0 ? nActive.ToString() : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    nTotalActive = nTotalActive + nActive;
                    nGrandTotalActive = nGrandTotalActive + nActive;

                    double nAbsent = oTempAMs.Sum(x => x.Absent);
                    _oPdfPCell = new PdfPCell(new Phrase(nAbsent > 0 ? nAbsent.ToString() : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    nTotalAbsent = nTotalAbsent + nAbsent;
                    nGrandTotalAbsent = nGrandTotalAbsent + nAbsent;

                    double nAbsentPercent = nAbsent / nActive * 100;
                    _oPdfPCell = new PdfPCell(new Phrase(nAbsentPercent > 0 ? Math.Round(nAbsentPercent,2).ToString(): "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    //nTotalAbsentPercent = nTotalAbsentPercent + nAbsentPercent;
                    //nGrandTotalAbsentPercent = nGrandTotalAbsentPercent + nAbsentPercent;

                    double nPresent = oTempAMs.Sum(x => x.Present);
                    _oPdfPCell = new PdfPCell(new Phrase(nPresent > 0 ? nPresent.ToString() : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    nTotalPresent = nTotalPresent + nPresent;
                    nGrandTotalPresent = nGrandTotalPresent + nPresent;

                    double nPresentPercent = nPresent / nActive * 100;
                    _oPdfPCell = new PdfPCell(new Phrase(nPresentPercent > 0 ? Math.Round(nPresentPercent,2).ToString() : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    //nTotalPresentPercent = nTotalPresentPercent + nPresentPercent;
                    //nGrandTotalPresentPercent = nGrandTotalPresentPercent + nPresentPercent;

                    _oPdfPTable.CompleteRow();

                    oTempAttendanceMonitorings.RemoveAll(x => x.DepartmentName == oTempAMs[0].DepartmentName);

                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (nTotalActive > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nTotalActive.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                if (nTotalAbsent > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nTotalAbsent.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nTotalAbsentPercent = nTotalAbsent / nTotalActive*100;
                if (nTotalAbsentPercent > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nTotalAbsentPercent,2).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                if (nTotalPresent > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nTotalPresent.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nTotalPresentPercent = nTotalPresent / nTotalActive * 100;
                if (nTotalPresentPercent > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nTotalPresentPercent,2).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPTable.CompleteRow();

                _oAttendanceMonitorings.RemoveAll(x => x.BUName == _oAttendanceMonitorings[0].BUName);

                 nTotalActive = 0;
                 nTotalPresent = 0;
                 nTotalPresentPercent = 0;
                 nTotalAbsent = 0;
                 nTotalAbsentPercent = 0;
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("GRAND TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (nGrandTotalActive > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(nGrandTotalActive.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nGrandTotalAbsent > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(nGrandTotalAbsent.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            nGrandTotalAbsentPercent = nGrandTotalAbsent / nGrandTotalActive * 100;
            if (nGrandTotalAbsentPercent > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nGrandTotalAbsentPercent,2).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            if (nGrandTotalPresent > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nGrandTotalPresent,2).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            nGrandTotalPresentPercent = nGrandTotalPresent / nGrandTotalActive * 100;
            if (nGrandTotalPresentPercent > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(nGrandTotalPresentPercent,2).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            
            _oPdfPTable.CompleteRow();
        }
        #endregion
    }

}
 