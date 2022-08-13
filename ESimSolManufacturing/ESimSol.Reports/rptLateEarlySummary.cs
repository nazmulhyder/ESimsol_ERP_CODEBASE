using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ESimSol.Reports
{

    public class rptLateEarlySummary
    {
        #region Declaration
        int count;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        //PdfPTable _oPdfPTableDetail = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendenceRegister _oAttendenceRegister = new AttendenceRegister();
        List<AttendenceRegister> _oAttendenceRegisters = new List<AttendenceRegister>();
        Company _oCompany = new Company();
        int nMidCol = 0;
        int nTotalColumn = 0;
        #endregion

        public byte[] PrepareReport(List<AttendenceRegister> oAttendenceRegisters, Company oCompany)
        {
            _oAttendenceRegisters = oAttendenceRegisters;
            _oAttendenceRegisters = _oAttendenceRegisters.OrderBy(x=>x.EmployeeID).ToList();
            _oCompany = oCompany;

            #region Page Setup
            if (_oAttendenceRegisters.Count > 0)
            {
                List<AttendenceRegister> distinctMonth = _oAttendenceRegisters
                                              .GroupBy(p => p.MonthID)
                                              .Select(g => g.First())
                                              .ToList();
                nMidCol = distinctMonth.Count;
            }
            int _nColumns = 5 + (nMidCol * 3) + 1 + 1;
            float[] tablecolumns = new float[_nColumns];

            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(5f, 5f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler


            _oDocument.Open();


            int nColumn = 0;

            tablecolumns[nColumn++] = 25f;
            tablecolumns[nColumn++] = 60f;
            tablecolumns[nColumn++] = 100f;
            tablecolumns[nColumn++] = 80f;
            tablecolumns[nColumn++] = 80f;

            if (nMidCol > 0)
            {
                int nRestWidth = 842 - 25 - 60 - 100 - 80 - 80 - 80 - 80;
                float nWidthForCol = (nRestWidth / nMidCol) / 3;
                for (int i = 0; i < nMidCol * 3; i++) 
                    tablecolumns[nColumn++] = nWidthForCol;
            }

            tablecolumns[nColumn++] = 80f;
            tablecolumns[nColumn++] = 80f;
            nTotalColumn = nColumn;
            _oPdfPTable.SetWidths(tablecolumns);

            //_oPdfPTable.SetWidths(new float[] {
            //                                25f,                                                  
            //                                60f,    //employee id
            //                                100f,   //full name
            //                                80f,    //designation
            //                                80f,    //department

                                            
            //                                80f,   //Total
            //                                80f,   //remarks
            //});
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 8;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Attendance Late and Early Summary", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Colspan = nTotalColumn;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = nTotalColumn;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = nTotalColumn;
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Quarterly Attendance Summary Report Late Attendance-Early Leave", _oFontStyle)); _oPdfPCell.Colspan = nTotalColumn;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            List<AttendenceRegister> distinctMonth = _oAttendenceRegisters
                                      .GroupBy(p => p.MonthID)
                                      .Select(g => g.First())
                                      .ToList();
            int nMidColCount = distinctMonth.Count();

            distinctMonth = distinctMonth.OrderBy(x => x.MonthID).ToList();

            string sAllMnths = "";
            string sYear = "";
            foreach (AttendenceRegister oAR in distinctMonth)
            {
                DateTime dtDate = new DateTime(oAR.YearID, oAR.MonthID, 1);
                string sMonthName = dtDate.ToString("MMM");

                dtDate = new DateTime(oAR.YearID, oAR.MonthID, 1);
                sYear = dtDate.ToString("yyyy");
                sAllMnths += sMonthName +", " ;
            }
            _oPdfPCell = new PdfPCell(new Phrase(sAllMnths + sYear, _oFontStyle)); _oPdfPCell.Colspan = nTotalColumn;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region header

            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee ID", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Full Name", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            foreach (AttendenceRegister oItem in distinctMonth)
            {
                DateTime dtDate = new DateTime(oItem.YearID, oItem.MonthID, 1);
                string sMonthName = dtDate.ToString("MMM");

                dtDate = new DateTime(oItem.YearID, oItem.MonthID, 1);
                sYear = dtDate.ToString("yy");

                _oPdfPCell = new PdfPCell(new Phrase(sMonthName + "-" + sYear, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("(Months)-Year", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle)); _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            int nTotalWD = 0;
            if (nMidColCount > 0)
            {
                foreach (AttendenceRegister oItem in distinctMonth)
                {
                    nTotalWD += oItem.TotalWorkingDays;
                    _oPdfPCell = new PdfPCell(new Phrase("Total Working Days(" + oItem.TotalWorkingDays + ")", _oFontStyle)); _oPdfPCell.Colspan = 3;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                }

            }
            _oPdfPCell = new PdfPCell(new Phrase("Total Working Days(" + nTotalWD + ")", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (nMidColCount > 0)
            {
                for (int i = 1; i <= nMidCol; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Late", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Early", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
            }
            _oPdfPCell = new PdfPCell(new Phrase("Total Att. Reprot", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            GetMonthWiseTable(_oAttendenceRegisters);
        }
        #endregion

        #region function

        #region Month wise
        private void GetMonthWiseTable(List<AttendenceRegister> oAttendenceRegisters)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            int nGrandTotal = 0;
            if (oAttendenceRegisters.Count > 0)
            {
                #region group by
                var data = oAttendenceRegisters.GroupBy(x => new { x.Code}, (key, grp) => new
                {
                    Code = key.Code,
                    Results = grp.ToList() //All data
                }).OrderBy(x => x.Code);
                #endregion

                List<AttendenceRegister> distinctMonth = _oAttendenceRegisters
                                          .GroupBy(p => p.MonthID)
                                          .Select(g => g.First())
                                          .ToList();
                int nMidColCount = distinctMonth.Count();
                distinctMonth = distinctMonth.OrderBy(x => x.MonthID).ToList();
                #region body

                count = 0;
                foreach (var oData in data)
                {
                    
                    #endregion

                    #region data
                    var oResults = oData.Results.GroupBy(x => new { x.Code }, (key, grp) => new
                    {
                        Code = key.Code,
                        EmployeeID = grp.First().EmployeeID,
                        EmployeeName=grp.First().EmployeeName,
                        Designation = grp.First().Designation,
                        Department = grp.First().Department,
                        Results = grp.ToList() //All data
                    }).OrderBy(x => x.Code);

                    foreach (var oItem in oResults)
                    {
                        count++; //num++;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);

                        _oPdfPCell = new PdfPCell(new Phrase(count.ToString(), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Code, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeName, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Designation, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Department, _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        
                        if (nMidColCount > 0)
                        {
                            foreach (AttendenceRegister oDM in distinctMonth)
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(oData.Results.Where(x => x.MonthID == oDM.MonthID).Sum(x => x.LateAttendanceCount).ToString(), _oFontStyle));
                                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                                _oPdfPCell = new PdfPCell(new Phrase(oData.Results.Where(x => x.MonthID == oDM.MonthID).Sum(x => x.EarlyLeaveCount).ToString(), _oFontStyle));
                                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                                int nTotal = oData.Results.Where(x => x.MonthID == oDM.MonthID).Sum(x => x.LateAttendanceCount) + oData.Results.Where(x => x.MonthID == oDM.MonthID).Sum(x => x.EarlyLeaveCount);
                                nGrandTotal += nTotal;

                                _oPdfPCell = new PdfPCell(new Phrase((nTotal).ToString(), _oFontStyle));
                                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            }
                            //for (int i = 1; i <= nMidCol; i++)
                            //{
                            //    _oPdfPCell = new PdfPCell(new Phrase(oData.Results.Where(x=>x.MonthID==nMidCol).Sum(x=>x.LateAttendanceCount).ToString(), _oFontStyle));
                            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            //    _oPdfPCell = new PdfPCell(new Phrase(oItem.EarlyLeaveCount.ToString(), _oFontStyle));
                            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            //    _oPdfPCell = new PdfPCell(new Phrase((oItem.LateAttendanceCount + oItem.EarlyLeaveCount).ToString(), _oFontStyle));
                            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            //}
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(nGrandTotal.ToString(), _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nGrandTotal = 0;

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();
                    }
                    #endregion

                    //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = nTotalColumn; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPTable.CompleteRow();
                }
                #endregion
            }
                
            //return _oPdfPTable;
        }
        #endregion

        
        #endregion



    }
}
