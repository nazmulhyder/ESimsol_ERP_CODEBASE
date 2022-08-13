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
    public class rptLeaveLedger
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        int _nColumns = 0;
        int _nPageWidth = 0;
        int _npageHeight = 650;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        LeaveLedgerReport _oLeaveLedgerReport = new LeaveLedgerReport();
        List<LeaveLedgerReport> _oLeaveLedgerReports = new List<LeaveLedgerReport>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();

        Company _oCompany = new Company();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();

        string _sSessions = "";
        int _nLeaveHeadID = 0;
        string _sStartDate = "";
        string _sEndDate = "";
        bool _bDate = false;

        #endregion
        public byte[] PrepareReport(LeaveLedgerReport oLeaveLedgerReport)
        {
            _oLeaveLedgerReports = oLeaveLedgerReport.LeaveLedgerReports;
            _oLeaveHeads = oLeaveLedgerReport.LeaveHeads;
            _oBusinessUnits = oLeaveLedgerReport.BusinessUnits;
            _oCompany = oLeaveLedgerReport.Company;
            _nLeaveHeadID = Convert.ToInt32(oLeaveLedgerReport.ErrorMessage.Split('~')[0]);
            _sSessions = oLeaveLedgerReport.ErrorMessage.Split('~')[1];
            _sStartDate =  oLeaveLedgerReport.ErrorMessage.Split('~')[2];
            _sEndDate =  oLeaveLedgerReport.ErrorMessage.Split('~')[3];
            _bDate = Convert.ToBoolean(oLeaveLedgerReport.ErrorMessage.Split('~')[4]);

            if (_nLeaveHeadID > 0)
            {
                _oLeaveHeads = _oLeaveHeads.Where(x => x.LeaveHeadID == _nLeaveHeadID).ToList();
            }

            #region Page Setup
            _nColumns = _oLeaveHeads.Count*3 + 8;

            float[] tablecolumns = new float[_nColumns];

            _nPageWidth = _nColumns*60;
            tablecolumns[0] = 15f;
            tablecolumns[1] = 50f;
            tablecolumns[2] = 100f;
            tablecolumns[3] = 100f;
            tablecolumns[4] = 50f;
            int i = 0;
            for (i = 5;i < _nColumns-3; i++)
            {
                tablecolumns[i] = 35f;
            }
            tablecolumns[i++] = 35f;
            tablecolumns[i++] = 35f;
            tablecolumns[i++] = 35f;

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
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits.Count == 1 ? _oBusinessUnits[0].Name : _oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits.Count ==1 ? _oBusinessUnits[0].Address : _oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 30;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Leave Ledger(Session-"+_sSessions+")", _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase("Leave Report " + (_bDate ? ("@ " + _sStartDate + " - " + _sEndDate) : "") + (_sSessions !="" ? (" @ Session-"+ _sSessions) : ""), _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

   
        #region Report Body
        int nCount = 0;
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            if (_oLeaveLedgerReports.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("There is no data to print !!", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                int RowCount = 0;
                _oLeaveLedgerReports = _oLeaveLedgerReports.OrderBy(x => x.DepartmentName).ToList();
                while (_oLeaveLedgerReports.Count > 0)
                {
                    List<LeaveLedgerReport> oTempLeaveLedgerReports = new List<LeaveLedgerReport>();
                    List<LeaveLedgerReport> oTempLeaveLedgerReports_dept = new List<LeaveLedgerReport>();
                    oTempLeaveLedgerReports_dept = _oLeaveLedgerReports.Where(x => x.DepartmentName == _oLeaveLedgerReports[0].DepartmentName).OrderBy(x => x.EmployeeCode).ToList();
                    oTempLeaveLedgerReports = _oLeaveLedgerReports.Where(x => x.DepartmentName == _oLeaveLedgerReports[0].DepartmentName).OrderBy(x => x.EmployeeCode).ToList();
                    nCount = 0;
                    PrintHaedRow(oTempLeaveLedgerReports_dept[0]);
                    while (oTempLeaveLedgerReports.Count > 0)
                    {
                        List<LeaveLedgerReport> oTempEmpLedgers = new List<LeaveLedgerReport>();
                        oTempEmpLedgers = oTempLeaveLedgerReports.Where(x => x.EmployeeID == oTempLeaveLedgerReports[0].EmployeeID).OrderBy(x => x.EmployeeCode).ToList();
                        PrintLeaveLedger(oTempEmpLedgers);
                        oTempLeaveLedgerReports.RemoveAll(x => x.EmployeeID == oTempEmpLedgers[0].EmployeeID);
                        RowCount++;
                    }

                    _oLeaveLedgerReports.RemoveAll(x => x.DepartmentName == oTempLeaveLedgerReports_dept[0].DepartmentName);


                    if (RowCount ==30)
                    {
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        this.PrintHeader();
                    }
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion
        private void PrintHaedRow(LeaveLedgerReport oLLR)
        {
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Arial Black", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Department : " + oLLR.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("CODE", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oLeaveHeads = _oLeaveHeads.OrderBy(x => x.LeaveHeadID).ToList();
            foreach(LeaveHead oLH in _oLeaveHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oLH.Name, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Summary", _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            foreach (LeaveHead oLH in _oLeaveHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Total Leave", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Enjoyed", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Total Leave", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Enjoyed", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }

        double TotalLeave_Summery = 0;
        double TotalEnjoyed_Summery = 0;
        double TotalBalance_Summery = 0;

        double TotalEnjoyed_Full= 0;
        double TotalEnjoyed_Half= 0;
        double TotalEnjoyed_Short = 0;

        double TotalBalance_Full = 0;
        double TotalBalance_Half = 0;
        double TotalBalance_Short = 0;
        private void PrintLeaveLedger(List<LeaveLedgerReport> oLLRs)
        {
            TotalLeave_Summery = 0;
            TotalEnjoyed_Summery = 0;
            TotalBalance_Summery = 0;


            TotalEnjoyed_Full = 0;
            TotalEnjoyed_Half = 0;
            TotalEnjoyed_Short = 0;

            TotalBalance_Full = 0;
            TotalBalance_Half = 0;
            TotalBalance_Short = 0;
           
            LeaveLedgerReport oTempLLR = new LeaveLedgerReport();
            if (oLLRs.Count > 0)
            {
                oTempLLR = oLLRs[0];
            }

            nCount++;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempLLR.EmployeeCode, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempLLR.EmployeeName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempLLR.DesignationName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oTempLLR.JoiningDateInString, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            foreach (LeaveHead oLH in _oLeaveHeads)
            {
                bool bExist = false;
                foreach (LeaveLedgerReport oLLR in oLLRs)
                {
                    if (oLH.LeaveHeadID == oLLR.LeaveHeadID)
                    {
                        bExist = true;
                        IsExist(bExist, oLLR);
                        TotalLeave_Summery += oLLR.TotalLeave;
                        TotalEnjoyed_Summery += oLLR.Enjoyed;

                        TotalEnjoyed_Full += oLLR.Full_Enjoyed;
                        TotalEnjoyed_Half += oLLR.Half_Enjoyed;
                        TotalEnjoyed_Short += oLLR.Short_Enjoyed;


                        TotalBalance_Summery += oLLR.Balance;

                        TotalBalance_Full += oLLR.Full_Balance;
                        TotalBalance_Half += oLLR.Half_Balance;
                        TotalBalance_Short += oLLR.Short_Balance;
                    }
                }
                if (!bExist)
                {
                    LeaveLedgerReport oLLR = new LeaveLedgerReport();
                    IsExist(bExist, oLLR);
                }
            }

            if (TotalBalance_Half > 0)
            {
                TotalBalance_Full += (int)(TotalBalance_Half / 2);
                TotalBalance_Half = (TotalBalance_Half % 2);
            }
            if (TotalBalance_Short > 0)
            {
                TotalBalance_Full += (int)(TotalBalance_Short / 3);
                TotalBalance_Short = (TotalBalance_Short % 3);
            }
            _oPdfPCell = new PdfPCell(new Phrase(TotalLeave_Summery.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               
            //TotalEnjoyed_Summery.ToString()
            _oPdfPCell = new PdfPCell(new Phrase("F:" + this.TotalEnjoyed_Full + "-H:" + this.TotalEnjoyed_Half + "-S:" + this.TotalEnjoyed_Short, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //TotalBalance_Summery.ToString()
            _oPdfPCell = new PdfPCell(new Phrase("F:" + this.TotalBalance_Full + "-H:" + this.TotalBalance_Half + "-S:" + this.TotalBalance_Short, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            
        }

        public void IsExist(bool bExist, LeaveLedgerReport oLLR)
        {
            if (bExist)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oLLR.TotalLeave.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oLLR.EnjoyedInfo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oLLR.BalanceInfo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
        }
    }
}
