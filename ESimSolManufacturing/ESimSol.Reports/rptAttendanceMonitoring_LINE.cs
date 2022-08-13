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
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptAttendanceMonitoring_LINE
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        AttendanceMonitoring _oAttendanceMonitoring = new AttendanceMonitoring();
        List<AttendanceMonitoring> _oAttendanceMonitorings = new List<AttendanceMonitoring>();
        List<AttendanceMonitoring> _oAttendanceMonitorings_DepSecs = new List<AttendanceMonitoring>();
        Company _oCompany = new Company();
        Dictionary<int, int> _departmentDesingnation = new Dictionary<int, int>();
        Dictionary<int, string> _departments = new Dictionary<int, string>();
        List<AttendanceMonitoring> _oAttendanceMonitorings_Des = new List<AttendanceMonitoring>();//distinct
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        int _nColumns = 0;
        int _maxDesignation = 0;
        #endregion

        public byte[] PrepareReport(AttendanceMonitoring oAttendanceMonitoring, List<BusinessUnit> oBusinessUnits)
        {
            _oAttendanceMonitorings = oAttendanceMonitoring.AttendanceMonitorings;
            _oAttendanceMonitorings_DepSecs = oAttendanceMonitoring.AttendanceMonitorings_DepSec;
            _oCompany = oAttendanceMonitoring.Company;

            _oBusinessUnits = oBusinessUnits;
            _oAttendanceMonitorings_Des = _oAttendanceMonitorings.GroupBy(p => p.DesignationID).Select(g => g.First()).ToList();

            _oAttendanceMonitorings.GroupBy(x => x.DepartmentID).Select(g => g.First()).ToList().ForEach(x =>
            {
                _departments.Add(x.DepartmentID, x.DepartmentName);
                _departmentDesingnation.Add(x.DepartmentID, _oAttendanceMonitorings_Des.Where(p => p.DepartmentID == x.DepartmentID).Select(o => o.DesignationID).Distinct().Count());
            });

            if (_departmentDesingnation.Count>0) _maxDesignation = _departmentDesingnation.Max(x => x.Value);

            #region Page Setup
            //_nColumns = _oAttendanceMonitorings_Des.Count * 4 + 3;
            _nColumns = _maxDesignation * 4 + 3;

            float[] tablecolumns = new float[_nColumns];

            tablecolumns[0] = 15f;
            tablecolumns[1] = 60f;

            int i = 2;
            for (int n = 0; n < 4; n++)
            {
                for (int m = 0; m < _maxDesignation; m++)
                {
                    tablecolumns[i++] = 25;
                }

                //foreach (AttendanceMonitoring oDesignation in _oAttendanceMonitorings_Des)
                //{
                //    tablecolumns[i++]= 25; 
                //}
            }
            tablecolumns[i++] = 25;

            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(1100, 700), 0f, 0f, 0f, 0f);

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
            if (_oBusinessUnits.Count > 1 || _oBusinessUnits.Count == 0)
            {
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
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnits[0].Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("ATTENDANCE MONITORING", _oFontStyle));
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
            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(GetDepSec());
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oAttendanceMonitorings.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to print!!", _oFontStyle));
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
                int nCount = 0;
                foreach (int nDeptId in _departmentDesingnation.Keys)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("DEPARTMENT NAME", _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PRESENT", _oFontStyle)); _oPdfPCell.Colspan = _maxDesignation;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("ABSENT", _oFontStyle)); _oPdfPCell.Colspan = _maxDesignation;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LEAVE", _oFontStyle)); _oPdfPCell.Colspan = _maxDesignation;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = _maxDesignation;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    /*----------------------- Second ----------------------*/

                    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_departments[nDeptId], _oFontStyle)); 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


                    for (int n = 0; n < 4; n++)
                    {
                        int nDesigCount = 0;
                        foreach (AttendanceMonitoring oDesignation in _oAttendanceMonitorings_Des.Where(x => x.DepartmentID == nDeptId).ToList())
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oDesignation.DesignationName, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                            nDesigCount++;
                        }
                        for(int x=0;x<_maxDesignation-nDesigCount;x++)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                    }
                    _oPdfPCell = new PdfPCell(new Phrase("COMMENT", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                                        /*-------------- Code For Line -----------------*/

                    List<AttendanceMonitoring> oTempAttendanceMonitorings = new List<AttendanceMonitoring>();
                    oTempAttendanceMonitorings = _oAttendanceMonitorings.Where(x => x.DepartmentID == nDeptId).ToList();

                    Dictionary<int, string> blockInfo = new Dictionary<int, string>();
                    Dictionary<string, int> summary = new Dictionary<string, int>();

                    oTempAttendanceMonitorings.GroupBy(x => x.BlockID).Select(g => g.First()).OrderBy(x => x.BlockID).ToList().ForEach(x =>
                    {
                        blockInfo.Add(x.BlockID, x.BlockName);
                    });

                    nCount = 0;
                    foreach (int blockId in blockInfo.Keys)
                    {
                        var attMonitorings = oTempAttendanceMonitorings.Where(x => x.BlockID == blockId).ToList();
                        ++nCount;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(blockInfo[blockId], _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        for (int n = 0; n < 4; n++)
                        {
                            int nDesigCount = 0;
                            foreach (AttendanceMonitoring oDesignation in _oAttendanceMonitorings_Des.Where(x => x.DepartmentID == nDeptId).ToList())
                            {
                                if (n == 0)// Present
                                {
                                    int nPresent = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Present);
                                    string key = "P" + oDesignation.DesignationID.ToString();
                                    if (summary.ContainsKey(key))
                                        summary[key] = summary[key] + nPresent;
                                    else
                                        summary[key] = nPresent;

                                    _oPdfPCell = new PdfPCell(new Phrase(nPresent.ToString(), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                }
                                if (n == 1)// Absent
                                {
                                    int nAbsent = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Absent);
                                    string key = "A" + oDesignation.DesignationID.ToString();
                                    if (summary.ContainsKey(key))
                                        summary[key] = summary[key] + nAbsent;
                                    else
                                        summary[key] = nAbsent;

                                    _oPdfPCell = new PdfPCell(new Phrase(nAbsent.ToString(), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                }
                                if (n == 2)// Leave
                                {
                                    int nLeave = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Leave);
                                    string key = "L" + oDesignation.DesignationID.ToString();
                                    if (summary.ContainsKey(key))
                                        summary[key] = summary[key] + nLeave;
                                    else
                                        summary[key] = nLeave;

                                    _oPdfPCell = new PdfPCell(new Phrase(nLeave.ToString(), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                }
                                if (n == 3)// Total
                                {
                                    int nTotal = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Present + x.Absent + x.Leave);
                                    string key = "T" + oDesignation.DesignationID.ToString();
                                    if (summary.ContainsKey(key))
                                        summary[key] = summary[key] + nTotal;
                                    else
                                        summary[key] = nTotal;

                                    _oPdfPCell = new PdfPCell(new Phrase(nTotal.ToString(), _oFontStyle));
                                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                }
                                nDesigCount++;
                            }
                            
                            for (int x = 0; x < _maxDesignation - nDesigCount; x++)
                            {
                                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            }
                        }
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                }
            }
        }
        #endregion

        public PdfPTable GetDepSec()
        {
            PdfPTable oPdfPTable;
            PdfPCell oPdfPCell;

            int maxSubSection = 0;
            if (_oAttendanceMonitorings_DepSecs.Count > 0)
            {
                maxSubSection = (from am in _oAttendanceMonitorings_DepSecs
                                 group am by am.DepartmentID into grp
                                 select new
                                 {
                                     DepartmentID = grp.Key,
                                     SectionCount = _oAttendanceMonitorings_DepSecs.Where(x => x.DepartmentID == grp.Key).ToList().Count()
                                 }).ToList().Max(x => x.SectionCount);
            }

            int nColumns = 4 + maxSubSection * 4 + 1;
            oPdfPTable = new PdfPTable(nColumns);
            float[] tablecolumns = new float[nColumns];

            tablecolumns[0] = 15f;
            tablecolumns[1] = 60f;

            int n = 0;
            for (n = 2; n < nColumns - 1; n++)
            {
                foreach (AttendanceMonitoring oDesignation in _oAttendanceMonitorings_DepSecs)
                {
                    tablecolumns[n] = 25; 
                }
            }
            tablecolumns[n] = 25;

            oPdfPTable.SetWidths(tablecolumns);


            if (_oAttendanceMonitorings_DepSecs.Count <= 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Colspan = nColumns; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Section", _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Present", _oFontStyle)); oPdfPCell.Colspan = maxSubSection; 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Absent", _oFontStyle)); oPdfPCell.Colspan = maxSubSection;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Leave", _oFontStyle)); oPdfPCell.Colspan = maxSubSection;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle)); oPdfPCell.Colspan = maxSubSection;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                List<AttendanceMonitoring> oAM_Depts = new List<AttendanceMonitoring>();
                oAM_Depts = _oAttendanceMonitorings_DepSecs.GroupBy(x => x.DepartmentID).Select(x => x).FirstOrDefault().ToList();

                foreach (AttendanceMonitoring oitem in oAM_Depts)
                {
                    oPdfPCell = new PdfPCell(new Phrase(oitem.DepartmentName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    string Sections = string.Join(",", _oAttendanceMonitorings_DepSecs.Where(x => x.DepartmentID == oitem.DepartmentID).Select(x => x.BlockName));
                    oPdfPCell = new PdfPCell(new Phrase(Sections, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    List<AttendanceMonitoring> oAM_Secs = new List<AttendanceMonitoring>();
                    oAM_Secs = _oAttendanceMonitorings_DepSecs.Where(x => x.DepartmentID == oitem.DepartmentID && x.Status == "P").OrderBy(x => x.BlockName).ToList();

                    foreach (AttendanceMonitoring oAMItem in oAM_Secs)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oAMItem.Count.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    }

                    oAM_Secs = new List<AttendanceMonitoring>();
                    oAM_Secs = _oAttendanceMonitorings_DepSecs.Where(x => x.DepartmentID == oitem.DepartmentID && x.Status == "A").OrderBy(x => x.BlockName).ToList();

                    foreach (AttendanceMonitoring oAMItem in oAM_Secs)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oAMItem.Count.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    }

                    oAM_Secs = new List<AttendanceMonitoring>();
                    oAM_Secs = _oAttendanceMonitorings_DepSecs.Where(x => x.DepartmentID == oitem.DepartmentID && x.Status == "L").OrderBy(x => x.BlockName).ToList();

                    foreach (AttendanceMonitoring oAMItem in oAM_Secs)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oAMItem.Count.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    }

                    List<AttendanceMonitoring> oAM_SecTotal = new List<AttendanceMonitoring>();
                    oAM_SecTotal = _oAttendanceMonitorings_DepSecs.Where(x => x.DepartmentID == oitem.DepartmentID).OrderBy(x => x.BlockName).ToList();
                    foreach (AttendanceMonitoring oAMItem in oAM_SecTotal)
                    {
                        int nTotal = 0;
                        nTotal = oAM_SecTotal.Where(x => x.BlockID == oAMItem.BlockID).Sum(x => x.Count);
                        oPdfPCell = new PdfPCell(new Phrase(nTotal.ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    }

                    if (oAM_SecTotal.Count > 0 && oAM_SecTotal.Count < maxSubSection)
                    {
                        for (int i = 0; i < (maxSubSection - oAM_SecTotal.Count); i++)
                        {
                            oPdfPCell = new PdfPCell(new Phrase(0.ToString(), _oFontStyle));
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                        }
                    }

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                }


            }
            return oPdfPTable;
        }

    }
}
