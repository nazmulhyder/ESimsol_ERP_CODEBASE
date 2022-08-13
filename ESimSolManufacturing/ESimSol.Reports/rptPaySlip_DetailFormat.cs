using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core.Framework;

namespace ESimSol.Reports
{
    public class rptPaySlip_DetailFormat
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        string _dataHeader = "";

        MemoryStream _oMemoryStream = new MemoryStream();
        EmployeeSalary _oEmployeeSalary = new EmployeeSalary();
        List<EmployeeSalary> _oEmployeeSalarys = new List<EmployeeSalary>();
        List<EmployeeSalaryDetail> _oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<EmployeeSalaryDetailDisciplinaryAction> _oEmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();
        Company _oCompany = new Company();
        List<EmployeeBankAccount> _oEmployeeBankAccounts = new List<EmployeeBankAccount>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        bool _bGroupBySerial = false;
        bool _bGroupByDept = false;

        string sHeader = "";

        #endregion

        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary, bool bGroupBySerial, bool bGroupByDept)
        {
            _bGroupBySerial = bGroupBySerial;
            _bGroupByDept = bGroupByDept;
            _dataHeader = oEmployeeSalary.Params;
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeSalaryID).ToList(); ;
            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalaryDetails;
            _oEmployeeSalaryDetailDisciplinaryActions = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            _oCompany = oEmployeeSalary.Company;
            _oEmployeeBankAccounts = oEmployeeSalary.EmployeeBankAccounts;
            _oBusinessUnits = oEmployeeSalary.BusinessUnits;

            if (_oEmployeeSalarys.Count > 0)
            {
                //DateTime dDateFrom = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[0]);
                //DateTime dDateTo = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[1]);
                string sDateFrom = _oEmployeeSalarys[0].StartDate.ToString("MMM") + ", " + _oEmployeeSalarys[0].StartDate.Year.ToString();
                string sDateTo = _oEmployeeSalarys[0].EndDate.ToString("MMM") + ", " + _oEmployeeSalarys[0].EndDate.Year.ToString();
                sHeader = "(" + sDateFrom + " To " + sDateTo+ ")";
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 30f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f, 100f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(1);
            oPdfPTableHeader.SetWidths(new float[] { 545f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(115f, 30f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 20;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.Border = 0;
                oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits.Count == 1 ? _oBusinessUnits[0].Name : _oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oBusinessUnits.Count == 1 ? _oBusinessUnits[0].Address : _oCompany.Address, _oFontStyle));
            //oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            if (_oEmployeeSalarys.Count() > 0)
            {
                while (_oEmployeeSalarys.Count() > 0)
                {
                    var oResults = new List<EmployeeSalary>();
                    if (_bGroupByDept && _bGroupBySerial)
                    {
                        oResults = _oEmployeeSalarys.Where(x => x.LocationID == _oEmployeeSalarys[0].LocationID && x.DepartmentID == _oEmployeeSalarys[0].DepartmentID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();

                    }
                    else if (_bGroupByDept && (_bGroupBySerial == false))
                    {
                        oResults = _oEmployeeSalarys.Where(x => x.LocationID == _oEmployeeSalarys[0].LocationID && x.DepartmentID == _oEmployeeSalarys[0].DepartmentID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();


                    }
                    else
                    {
                        oResults = _oEmployeeSalarys.Where(x => x.LocationID == _oEmployeeSalarys[0].LocationID).OrderBy(x => x.LocationName).ThenBy(x => x.EmployeeCode).ToList();

                    }
                    //oResults = _oEmployeeSalarys.Where(x => x.LocationID == _oEmployeeSalarys[0].LocationID && x.DepartmentID == _oEmployeeSalarys[0].DepartmentID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();

                    //if (_bGroupBySerial)
                    //{
                    //    N = 0;
                    //}
                    BodyReapeat(oResults);
                    if (_bGroupByDept)
                        _oEmployeeSalarys.RemoveAll(x => x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                    else
                        _oEmployeeSalarys.RemoveAll(x => x.LocationID == oResults[0].LocationID);
                }
                //foreach (EmployeeSalary oESItem in _oEmployeeSalarys)
                //{
                //    PrintHeader();
                //    BodyReapeat(oESItem);
                //    _oDocument.Add(_oPdfPTable);
                //    _oDocument.NewPage();
                //    _oPdfPTable.DeleteBodyRows();
                //}
            }
            else
            {
                PrintHeader();
                _oPdfPCell = new PdfPCell(new Phrase("No Information Found.", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40; _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }

        private void BodyReapeat(List<EmployeeSalary> oESItems)
        {
            foreach (EmployeeSalary oESItem in oESItems)
            {
                PrintHeader();

                double nTotalAddition = 0;
                double nTotalDeduction = 0;
                int nFixedHeight = 20;

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Pay Slip", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_dataHeader, _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                _oPdfPCell = new PdfPCell(new Phrase("CONFIDENTIAL", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                
                //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                //_oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 80f, 120f, 150f, 150f });
                PdfPCell oPdfPCell;

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("Employee Code", _oFontStyle)); oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase(": " + oESItem.EmployeeCode, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("Employee Name", _oFontStyle)); oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(": " + oESItem.EmployeeName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(": " + oESItem.DesignationName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("Date of Joining", _oFontStyle)); oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(": " + oESItem.JoiningDateInString, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("Location", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(": " + oESItem.LocationName, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("ETIN", _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPCell = new PdfPCell(new Phrase(": " + oESItem.ETIN, _oFontStyle)); oPdfPCell.Border = 0; oPdfPCell.Colspan = 3;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("Earnings", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Deductions", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("Heads", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount (BDT)", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Heads", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount (BDT)", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

                List<EmployeeSalaryDetail> oTempESDetails = new List<EmployeeSalaryDetail>();
                oTempESDetails = _oEmployeeSalaryDetails.Where(x => x.EmployeeSalaryID == oESItem.EmployeeSalaryID).OrderBy(x => x.SalaryHeadID).ToList();


                List<EmployeeSalaryDetail> oTempESDetailAdditions = new List<EmployeeSalaryDetail>();
                List<EmployeeSalaryDetail> oTempESDetailDeductions = new List<EmployeeSalaryDetail>();
                oTempESDetailAdditions.AddRange(oTempESDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Basic)).ToList());

                EmployeeSalaryDetail oTempESD = new EmployeeSalaryDetail();
                oTempESD.SalaryHeadName = "Gross";
                oTempESD.Amount = oESItem.GrossAmount;
                //oTempESDetailAdditions.Add(oTempESD);           Gross Add here

                oTempESDetailAdditions.AddRange(oTempESDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).ToList());
                oTempESDetailDeductions.AddRange(oTempESDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).ToList());
                

                int nLarge = 0;
                int AdditionRows = 0;
                int DeductionRows = 0;

                AdditionRows = oTempESDetailAdditions.Count;
                DeductionRows = oTempESDetailDeductions.Count;

                if (AdditionRows > DeductionRows)
                {
                    nLarge = AdditionRows;
                }
                else
                {
                    nLarge = DeductionRows;
                }

                for (int i = 0; i < nLarge; i++)
                {
                    if (oTempESDetailAdditions.Count > i)
                    {
                        if (oTempESDetailAdditions[i].SalaryHeadName == "Gross")
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                            nTotalAddition -= oTempESDetailAdditions[i].Amount;
                        }
                        else
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(oTempESDetailAdditions[i].SalaryHeadName, _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oTempESDetailAdditions[i].Amount), _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        nTotalAddition += oTempESDetailAdditions[i].Amount;
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    if (oTempESDetailDeductions.Count > i)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oTempESDetailDeductions[i].SalaryHeadName, _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oTempESDetailDeductions[i].Amount), _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        nTotalDeduction += oTempESDetailDeductions[i].Amount;
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        nTotalDeduction += 0;
                    }
                    _oPdfPTable.CompleteRow();
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("Total Earnings", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //List<EmployeeSalaryDetail>  oEmpSDs = new List<EmployeeSalaryDetail>();
                //oEmpSDs =  _oEmployeeSalaryDetails.Where(x => ((x.SalaryHeadType == (int)EnumSalaryHeadType.Basic) || (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition) && x.SalaryHeadName != "Gross")).ToList();
                //nTotalAddition = oEmpSDs.Sum(x => x.Amount);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAddition), _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Deductions", _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeduction), _oFontStyle)); _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("Net Payable", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(((nTotalAddition - nTotalDeduction) < 0) ? "(" + Global.MillionFormat((nTotalAddition - nTotalDeduction) * (-1)) + ")" : Global.MillionFormat(nTotalAddition - nTotalDeduction), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

                List<EmployeeBankAccount> oEmployeeBankAccounts = new List<EmployeeBankAccount>();
                oEmployeeBankAccounts = _oEmployeeBankAccounts.Where(x => x.EmployeeID == oESItem.EmployeeID).ToList();
                string sAccountNo = string.Join(",", oEmployeeBankAccounts.Select(x => x.AccountNo));

                //string sBanks = string.Join(",", oEmployeeBankAccounts.Select(x => x.BankBranchName));
                //_oPdfPCell = new PdfPCell(new Phrase("Amount Credited To: " + sBanks, _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = nFixedHeight;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 20;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPTable.CompleteRow();

                double nAmt = nTotalAddition - nTotalDeduction - oESItem.CashAmount;

                if (nAmt > 0 || oESItem.CashAmount > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("MODE OF PAYMENT", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = nFixedHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    if (oESItem.NetAmount > 0)
                    {
                        if (nAmt > 0)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oPdfPCell = new PdfPCell(new Phrase("Through Bank Account: (" + sAccountNo + ")", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAddition - nTotalDeduction - oESItem.CashAmount), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }

                        //if (oESItem.NetAmount > 0)
                        //{

                        //    _oPdfPCell = new PdfPCell(new Phrase((Global.MillionFormat(nTotalAddition - nTotalDeduction - oESItem.CashAmount), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase((0.0).ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //}
                    }

                    if (oESItem.NetAmount > 0)
                    {
                        if (oESItem.CashAmount > 0)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oPdfPCell = new PdfPCell(new Phrase("Through Cash Payment/ Cheque: ", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                            _oPdfPCell = new PdfPCell(new Phrase((oESItem.CashAmount).ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }

                        

                        //if (oESItem.NetAmount > 0)
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase((oESItem.CashAmount).ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase((0.0).ToString(), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = nFixedHeight;
                        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        //}
                    }

                }

                

                

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 100; _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("N.B. This Electronic Document does not require any signature", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                nTotalAddition = 0;

                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
            }

        }
        #endregion


        //public double GetAbsentDeduction(int nEmployeeSalaryID)
        //{
        //    double nAbsentDeduction = 0;
        //    foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
        //    {
        //        if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "UnPaidLeave")
        //        {
        //            nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;
        //        }
        //    }
        //    return nAbsentDeduction;
        //}
        //public double GetLoanDeduction(int nEmployeeSalaryID)
        //{
        //    double nAbsentDeduction = 0;
        //    foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
        //    {
        //        if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "Loan")
        //        {
        //            nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;
        //        }
        //    }
        //    return nAbsentDeduction;
        //}
        //public double GetAdvanceDeduction(int nEmployeeSalaryID)
        //{
        //    double nAbsentDeduction = 0;
        //    foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
        //    {
        //        if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "Advance")
        //        {
        //            nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;
        //        }
        //    }
        //    return nAbsentDeduction;
        //}
    }
}
