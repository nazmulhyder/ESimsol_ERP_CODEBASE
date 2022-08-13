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
    public class rec
    {
        public string DepartmentName { get; set; }
        public int NoOfEmployee { get; set; }
    }
    public class rptNewEmployeeListWithSummery
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPCell _oPdfPCell;

        MemoryStream _oMemoryStream = new MemoryStream();
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        List<Department> _oDepartments = new List<Department>();
        Company _oCompany = new Company();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        List<rec> showSummery = new List<rec>();


        string sHeader = "";

        #endregion

        public byte[] PrepareReport(Employee oEmployee, List<BusinessUnit> oBusinessUnits)
        {
            _oEmployees = oEmployee.Employees;
            _oDepartments = oEmployee.Departments;
            _oCompany = oEmployee.Company;
            _oBusinessUnits = oBusinessUnits;

            DateTime dDateFrom = Convert.ToDateTime(oEmployee.ErrorMessage.Split('~')[0]);
            DateTime dDateTo = Convert.ToDateTime(oEmployee.ErrorMessage.Split('~')[1]);
            string sDateFrom = dDateFrom.ToString("dd MMM yyyy");
            string sDateTo = dDateTo.ToString("dd MMM yyyy");
            sHeader = "Date : From " + sDateFrom + " To " + sDateTo;

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
            _oPdfPTable.SetWidths(new float[] { 25f, 60f, 100f, 100f, 50f, 50f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            this.PrintFooter();
            //_oPdfPTable.HeaderRows = 7;
            //_oPdfPTable.FooterRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(3);
            oPdfPTableHeader.SetWidths(new float[] { 160f, 130f,125f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(20f, 15f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = 2;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.Colspan = 3; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits.Count == 1 ? _oBusinessUnits[0].Name : _oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            /*_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCellHearder = new PdfPCell(new Phrase("BGMEA Reg. No.-" + _oCompany.CompanyRegNo, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            */
            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits.Count == 1 ? _oBusinessUnits[0].Address : _oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("NEW EMPLOYEE LIST", _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 6;
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
            


            int RowCount = 0;
            int i = 1;

            foreach (Department oDepartmentItem in _oDepartments)
            {
                RowCount++;
                RowCount++;
                rec record = new rec();

                List<Employee> oEmployees = (from oEs in _oEmployees

                                             where (oEs.DepartmentID == oDepartmentItem.DepartmentID)

                                             select oEs).ToList();
                if (oEmployees.Count > 0)
                {

                    record.NoOfEmployee = oEmployees.Count();
                    record.DepartmentName = oEmployees[0].DepartmentName;
                    showSummery.Add(record);
                }

                if (oEmployees.Count > 0)
                {
                    HeaderRow(oEmployees[0]);
                    int nCount = 0;
                    
                    foreach (Employee oEmployeeItem in oEmployees)
                    {
                        nCount++;
                        RowCount++;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oEmployeeItem.Code, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oEmployeeItem.Name, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oEmployeeItem.DesignationName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oEmployeeItem.DateOfBirthInString, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oEmployeeItem.DateOfJoinInString, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();

                        if (RowCount%58 == 0)
                        {
                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            _oPdfPTable.DeleteBodyRows();
                            this.PrintHeader();
                            this.HeaderRow(oEmployeeItem);
                        }

                    }
                }
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();

            this.PrintHeader();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SUMMARY", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("No of Person", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            List<Employee> oEmps = new List<Employee>();
            var BUIDs = _oEmployees.Select(p => p.BusinessUnitID).Distinct().ToList();
            foreach (var buid in BUIDs)
            {
                var oLocIDs = _oEmployees.Where(x => x.BusinessUnitID == buid).Select(x => x.LocationID).Distinct().ToList();
                foreach (var locid in oLocIDs)
                {
                    var oDepts = _oEmployees.Where(x => x.LocationID == locid).Select(x => x.DepartmentID).Distinct().ToList();
                    if (oDepts.Count > 0)
                    {

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("BU Name : " + _oEmployees.Where(x => x.BusinessUnitID == buid).ToList().First().BUName + " , Location Name: " + _oEmployees.Where(x => x.LocationID == locid).ToList().First().LocationName, _oFontStyle)); _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();

                        foreach (var oDeptID in oDepts)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            var tempSummery = _oEmployees.Where(x => x.DepartmentID == oDeptID && x.BusinessUnitID == buid && x.LocationID == locid).ToList();
                            _oPdfPCell = new PdfPCell(new Phrase(tempSummery.First().DepartmentName, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(tempSummery.Count().ToString(), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPTable.CompleteRow();

                        }
                    }
                }
            }

            //foreach (rec oRec in showSummery)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase(oRec.DepartmentName, _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase(oRec.NoOfEmployee.ToString(), _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPTable.CompleteRow();

            //}

            double nTotalEmp= showSummery.Sum(x=>x.NoOfEmployee);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalEmp.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            //if (RowCount < 40 || RowCount < 40 * i)
            //{
            //    PrintFooter();
            //}


        }
        #endregion

        public void HeaderRow(Employee oEmployee)
        {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("SEC : " + oEmployee.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("EMPLOYEE ID", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DESIGNATION", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DATE OF BIRTH", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("JOIN DATE", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPTable.CompleteRow();
        }

        #region Report Footer
        private void PrintFooter()
        {
            #region CompanyFooter


            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("SIGNATURE", _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

    }

}
