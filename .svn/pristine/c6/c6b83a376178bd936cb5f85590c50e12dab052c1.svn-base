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
    public class rptBOAEL
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
        BenefitOnAttendanceEmployeeLedger _oBOAEL = new BenefitOnAttendanceEmployeeLedger();
        List<BenefitOnAttendanceEmployeeLedger> _oBOAELs = new List<BenefitOnAttendanceEmployeeLedger>();
        Company _oCompany = new Company();

        string sHeader = "";
        DateTime _sStartDate = DateTime.Now;
        DateTime _sEndDate = DateTime.Now;
        int _nDays = 0;
        int _ColSpan1 = 0;

        #endregion

        public byte[] PrepareReport(BenefitOnAttendanceEmployeeLedger BOAEL)
        {
            _oBOAELs = BOAEL.BOAELs;
            _oCompany = BOAEL.Company;
            _sStartDate = Convert.ToDateTime(BOAEL.ErrorMessage.Split('~')[0]);
            _sEndDate = Convert.ToDateTime(BOAEL.ErrorMessage.Split('~')[1]);
            if (_sStartDate.Month == _sEndDate.Month)
            {
                _ColSpan1 = _sEndDate.Day - _sStartDate.Day + 1;
            }
            else
            {
                _ColSpan1 = DateTime.DaysInMonth(_sStartDate.Year, _sStartDate.Month) - Convert.ToInt32(_sStartDate.Day) + 1;
            }
            if(_oBOAELs.Count>0)
            {
                sHeader=_oBOAELs[0].BOAName;
            }
         
            #region Page Setup
            TimeSpan diff = _sEndDate - _sStartDate;
            _nDays = diff.Days + 1;
            _nColumns = _nDays + 3;

            float[] tablecolumns = new float[_nColumns];

            if (_nColumns <= 12)
            {
                _nPageWidth = 500;
                tablecolumns[0] = 15f;
                tablecolumns[1] = 30f;
                tablecolumns[2] = 30f;
            }
            else
            {
                _nPageWidth = 40* (_nColumns);
                tablecolumns[0] = 15f;
                tablecolumns[1] = 30f;
                tablecolumns[2] = 30f;
            }

            for (int i = 3; i < _nColumns; i++)
            {
                tablecolumns[i] = 15f;
            }


            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
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
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
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
            _oPdfPCell.FixedHeight = 7;
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

            if (_oBOAELs.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("There is no data to print !!", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                _oBOAELs = _oBOAELs.OrderBy(x => x.DepartmentName).ToList();
                while (_oBOAELs.Count > 0)
                {
                    List<BenefitOnAttendanceEmployeeLedger> oTempBenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
                    oTempBenefitOnAttendanceEmployeeLedgers = _oBOAELs.Where(x => x.DepartmentName == _oBOAELs[0].DepartmentName).ToList();
                    _oBOAELs.RemoveAll(x => x.DepartmentName == oTempBenefitOnAttendanceEmployeeLedgers[0].DepartmentName);
                    PrintHaedRow(oTempBenefitOnAttendanceEmployeeLedgers[0]);
                    while (oTempBenefitOnAttendanceEmployeeLedgers.Count > 0)
                    {
                        List<BenefitOnAttendanceEmployeeLedger> oTempBOAELs = new List<BenefitOnAttendanceEmployeeLedger>();
                        oTempBOAELs = oTempBenefitOnAttendanceEmployeeLedgers.Where(x => x.EmployeeID == oTempBenefitOnAttendanceEmployeeLedgers[0].EmployeeID).ToList();
                        PrintBenefitOnAttendanceEmployeeLedger(oTempBOAELs);
                        oTempBenefitOnAttendanceEmployeeLedgers.RemoveAll(x => x.EmployeeID == oTempBOAELs[0].EmployeeID);
                    }
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion
        private void PrintHaedRow(BenefitOnAttendanceEmployeeLedger oAD)
        {
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Arial Black", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Department : " + oAD.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("CODE", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_sStartDate.ToString("MMM"), _oFontStyle)); _oPdfPCell.Colspan = _ColSpan1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; 
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (_sStartDate.Month != _sEndDate.Month)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_sEndDate.ToString("MMM"), _oFontStyle)); _oPdfPCell.Colspan = _sEndDate.Day;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();

            DateTime dStartDate = _sStartDate;
            while (dStartDate <= _sEndDate)
            {
                _oPdfPCell = new PdfPCell(new Phrase(dStartDate.Day.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                dStartDate = dStartDate.AddDays(1);
            }
            _oPdfPTable.CompleteRow();
        }

        private void PrintBenefitOnAttendanceEmployeeLedger(List<BenefitOnAttendanceEmployeeLedger> oADs)
        {
            int nCount = 0;
            foreach (BenefitOnAttendanceEmployeeLedger oItem in oADs)
            {
                nCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeCode, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeName, _oFontStyle)); _oPdfPCell.Rotation = 90;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                DateTime dStartDate = _sStartDate;
                while (dStartDate <= _sEndDate)
                {
                    if (dStartDate == oItem.AttendanceDate)
                    {
                        BOAELExist(oItem);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Rotation = 90;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    dStartDate = dStartDate.AddDays(1);
                }
                _oPdfPTable.CompleteRow();
            }
        }

        public void BOAELExist(BenefitOnAttendanceEmployeeLedger oItem)
        {
            string S = oItem.BOAName;
            //if (oItem.BenefitOn==EnumBenefitOnAttendance.Time_Slot)
            //{
            //    S += "(" + oItem.TimeSlotInString + ")";
            //}
            _oPdfPCell = new PdfPCell(new Phrase("Exist", _oFontStyle)); _oPdfPCell.Rotation = 90;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            
        }
    }

}
