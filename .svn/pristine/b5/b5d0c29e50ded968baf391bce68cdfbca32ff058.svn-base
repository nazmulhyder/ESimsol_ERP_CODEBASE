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
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Reports
{

    public class rptEmployeeSalary
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
        EmployeeSalary _oEmployeeSalary = new EmployeeSalary();
        List<EmployeeSalary> _oEmployeeSalarys = new List<EmployeeSalary>();

        Company _oCompany = new Company();
        List<SalaryHead> _oSalaryHeads = new List<SalaryHead>();
        List<EmployeeSalaryDetail> _oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<EmployeeSalaryDetailDisciplinaryAction> _oESDDAs = new List<EmployeeSalaryDetailDisciplinaryAction>();
        string _sStartDate = "";
        string _sEndDate = "";

        

        #endregion

        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary)
        {
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarys;
            _oSalaryHeads = oEmployeeSalary.SalaryHeads;
            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalaryDetails;
            _oESDDAs = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            _oCompany = oEmployeeSalary.Company;
            _sStartDate = oEmployeeSalary.ErrorMessage.Split(',')[0];
            _sEndDate = oEmployeeSalary.ErrorMessage.Split(',')[1];

           
            #region Page Setup
            _nColumns = _oSalaryHeads.Count + 5;

            float[] tablecolumns = new float[_nColumns];

            if (_nColumns <= 5)
            {
                _nPageWidth = 500;
                tablecolumns[0] = 20f;
                tablecolumns[1] = 130f;
            }
            else
            {
                _nPageWidth = 100 * (_nColumns);
                tablecolumns[0] = 15f;
                tablecolumns[1] = 120f;
            }

            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 75;
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
            _oPdfPTable.HeaderRows = 7;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 5;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Employee Salary", _oFontStyle));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From "+_sStartDate+" To "+_sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 5;
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("SL No.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            foreach (SalaryHead oItem in _oSalaryHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Others Addition", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Total Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);


            foreach (EmployeeSalary oEItem in _oEmployeeSalarys)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oEItem.EmployeeNameCode + ", " + oEItem.DesignationName + ", " + oEItem.DepartmentName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nTotal = 0;
                foreach (SalaryHead oItem in _oSalaryHeads)
                {

                    double nAm = 0;
                    nAm = GetAmount(oItem.SalaryHeadID, oEItem.EmployeeSalaryID);
                    nTotal = nTotal + nAm;
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAm), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                bool bAddition = false;
                bool bDeduction = false;
                double DeductionAmount = 0;

                foreach (EmployeeSalaryDetailDisciplinaryAction oESDDAItem in _oESDDAs)
                {
                    if (oESDDAItem.ActionName == "Addition" && oEItem.EmployeeSalaryID == oESDDAItem.EmployeeSalaryID)
                    {
                        bAddition = true;
                        nTotal = nTotal + oESDDAItem.Amount;

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oESDDAItem.Amount), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    else if (oESDDAItem.ActionName == "Deduction" && oEItem.EmployeeSalaryID == oESDDAItem.EmployeeSalaryID)
                    {
                        bDeduction = true;
                        nTotal = nTotal - oESDDAItem.Amount;
                        DeductionAmount = oESDDAItem.Amount;
                        if (bAddition != false)
                        {

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(DeductionAmount), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        }
                    }
                    if (oEItem.EmployeeSalaryID == oESDDAItem.EmployeeSalaryID)
                    {
                        break;
                    }

                }

                if (bAddition == true && bDeduction == false)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(0.0), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (bAddition == false && bDeduction == true)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(0.0), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(DeductionAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oESDDAs.Count <= 0)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(0.00), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(0.00), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                }

               
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotal), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

            }
        }

        public double GetAmount(int nSHID, int nESSID)
        {
            double nAmount = 0;
            foreach (EmployeeSalaryDetail oEDItem in _oEmployeeSalaryDetails)
            {
                if (oEDItem.SalaryHeadID == nSHID && oEDItem.EmployeeSalaryID == nESSID)
                {
                    nAmount = oEDItem.Amount;                    
                }

            }

            return nAmount;

        }

        #endregion
    }




}
