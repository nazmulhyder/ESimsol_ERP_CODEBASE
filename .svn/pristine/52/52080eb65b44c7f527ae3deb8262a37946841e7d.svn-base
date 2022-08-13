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
    public class rptBankSheet
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        int styleIDCount = 0;
        double nTableHeight = 87;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        List<SignatureSetup> oSignatureSetups;

        List<RPTSalarySheet> _oEmployeeSalarys = new List<RPTSalarySheet>();
        List<EmployeeBankAccount> _oEmployeeBankAccounts = new List<EmployeeBankAccount>();
        BankAccount _oBankAccount = new BankAccount();
        string[] _selectedColNames;
        string sColList = "";
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        DateTime _dApplyDate = DateTime.Now;
        bool _bHasOTAllowance = false;
        bool isRound = false;
        string _dateHeader = "";
        int _nMonthID = 0, _nYearID = 0;

        float _nHeaderHeight = 250f;
        float _nFullPageHeight = 840f;
        float _nFooterRequirHeight = 110f;
        string _sParamPrintHeader="";        
        #endregion

        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary, List<SalarySheetProperty> oSalarySheetPropertys, string SelectedColNames, bool bRound, BankAccount oBankAccount, DateTime dApplyDate, int nMonthID, int nYearID, List<SignatureSetup> oSignatureSetups, string sPrintHeader, float HeaderHeightInch, float FooterHeightInch)
        {
            isRound = bRound;
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarySheets;
            _dateHeader = oEmployeeSalary.Params;
            _oBankAccount = oBankAccount;
            _dApplyDate = dApplyDate;
            _oCompany = oEmployeeSalary.Company;
            sColList = SelectedColNames;
            _nMonthID = nMonthID;
            _oSignatureSetups = oSignatureSetups;
            _nYearID = nYearID;
            _sParamPrintHeader = sPrintHeader;           

            _nHeaderHeight = 60;
            _nFullPageHeight = 840;
            _nFooterRequirHeight = 20 + 50;//Here 20 for Footer & 50 for Signature
            if (_nHeaderHeight < (HeaderHeightInch * 72.03f)) //Here A4 size page 11.69" & 842 pixel  
            {
                _nHeaderHeight = (HeaderHeightInch * 72.03f);
            }
            if (_nFooterRequirHeight < (FooterHeightInch * 72.03f)) //Here A4 size page 11.7" & 840 pixel  
            {
                _nFooterRequirHeight = (FooterHeightInch * 72.03f);
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(30f, 30f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PageEventHandler.signatures = oSignatureSetups.Select(x => x.SignatureCaption).ToList();
            PageEventHandler.SignatureName = oSignatureSetups.Select(x => x.DisplayFixedName).ToList();
            PageEventHandler.PrintPrintingDateTime = true;
            PageEventHandler.nFontSize = 7;
            PDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595 });
            #endregion

            if (_sParamPrintHeader == "PrintWithPad")
            {
                PrintRow("", FontFactory.GetFont("Tahoma", 12f, 1), _nHeaderHeight, false);
            }
            else
            {
                PrintHeader();
            }
            PrintRow("Bank Payment Advice", FontFactory.GetFont("Tahoma", 12f, 1), 25, true);
            PrintBankPart();
            PrintBody();
            PrintLastPart();


            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeader()
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 180, 440, 180 });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion
        }
        private void PrintBankPart()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("Date: " + _dApplyDate.ToString("dd MMM yyyy"), _oFontStyleBold)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("To", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("The Branch Manager", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oBankAccount.BankNameBranch, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oBankAccount.BranchAddress, _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            string strMonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_nMonthID);
            _oPdfPCell = new PdfPCell(new Phrase("Subject: Request to disburse salary for the month of " + strMonthName + ", " + _nYearID + ".", FontFactory.GetFont("Tahoma", 9f, 1 | iTextSharp.text.Font.UNDERLINE))); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Dear Sir,", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            Phrase oPhrase = new Phrase();
            Chunk oChunk1 = new Chunk("We are requesting you to disburse the net payable salary BDT ", _oFontStyle); oPhrase.Add(oChunk1);
            Chunk oChunk2 = new Chunk(_oEmployeeSalarys.Sum(x => x.BankAmount).ToString("###,##0;(###,##0)"), _oFontStyleBold); oPhrase.Add(oChunk2);
            Chunk oChunk3 = new Chunk(" (In Words : ", _oFontStyle); oPhrase.Add(oChunk3);
            Chunk oChunk4 = new Chunk(Global.TakaWords(Math.Round(_oEmployeeSalarys.Sum(x => x.BankAmount), 0)), _oFontStyleBold); oPhrase.Add(oChunk4);
            Chunk oChunk5 = new Chunk(") from our", _oFontStyle); oPhrase.Add(oChunk5);
            if (_oCompany.BaseAddress.ToUpper() == "ISPAHANI")//ispahani
            {
                Chunk oChunk6 = new Chunk(" Hospital", _oFontStyle); oPhrase.Add(oChunk6);
            }
            else
            {
                Chunk oChunk7 = new Chunk(" Company", _oFontStyle); oPhrase.Add(oChunk7);
            }
            Chunk oChunk8 = new Chunk(" Account (" + _oBankAccount.AccountNo + ") to our employee’s individual accounts as mentioned here below :", _oFontStyle); oPhrase.Add(oChunk8);
            
            _oPdfPCell = new PdfPCell(oPhrase); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintBody()
        {
           
            if (sColList != "")
            {
                _selectedColNames = sColList.Split(',');
            }

            var matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeCode"));
            float[] setWidth = new float[_selectedColNames.Length + 1];
            setWidth[0] = 25;
            for(int i=1; i<=_selectedColNames.Length; i++)
            {                
                if (matchString != null && i == 1)
                {
                    setWidth[i] = 40; //for Employee Code
                }
                else if (i == 2)
                {
                    setWidth[i] = 80; //for Employee Name
                }
                else if (i == _selectedColNames.Length)
                {
                    setWidth[i] = 30; //For Amount
                }
                else
                {
                    setWidth[i] = 60;
                }
            }
            PdfPTable oHeaderPdfPTable = new PdfPTable(_selectedColNames.Length + 1);
            oHeaderPdfPTable.WidthPercentage = 100;
            oHeaderPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oHeaderPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oHeaderPdfPTable.SetWidths(setWidth);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, 1);

            #region Header
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold)); _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeCode"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeName"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DepartmentName"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DesignationName"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("JoiningDate"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("ContactNo"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Contact No", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }
            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("AccountNo"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Account No", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }
            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("Amount"));
            if (matchString != null){
                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oHeaderPdfPTable.AddCell(_oPdfPCell);
                matchString = null;
            }
            oHeaderPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oHeaderPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            int nCount = 0; double nTotalAmount = 0;
            foreach (RPTSalarySheet oItem in _oEmployeeSalarys)
            {

                PdfPTable oCPdfPTable = new PdfPTable(_selectedColNames.Length + 1);
                oCPdfPTable.WidthPercentage = 100;
                oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oCPdfPTable.SetWidths(setWidth);

                if (oItem.BankAmount > 0)
                {
                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(" " + nCount.ToString(), _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeCode"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.EmployeeCode, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("EmployeeName"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.EmployeeName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DepartmentName"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.DepartmentName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("DesignationName"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.DesignationName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("JoiningDate"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.JoiningDateInString, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("ContactNo"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.ContactNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("AccountNo"));
                    if (matchString != null)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oItem.AccountNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }

                    matchString = _selectedColNames.FirstOrDefault(x => x.Contains("Amount"));
                    if (matchString != null)
                    {
                        double nOTAllowance = Math.Round(oItem.OTAmount);
                        nTotalAmount += oItem.BankAmount;
                        _oPdfPCell = new PdfPCell(new Phrase((isRound == true) ? Math.Round(oItem.BankAmount, 0).ToString("###,##0;(#,##,##)") : oItem.BankAmount.ToString("###,##0;(#,##,##0)"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                        matchString = null;
                    }
                    oCPdfPTable.CompleteRow();

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oCPdfPTable);
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    double nYetToUsagesHeight = _nFullPageHeight - (CalculatePdfPTableHeight(_oPdfPTable) + _nFooterRequirHeight);
                    if (nYetToUsagesHeight < _nFooterRequirHeight)
                    {
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();

                        #region Add Header Table
                        if (_sParamPrintHeader == "PrintWithPad")
                        {
                            PrintRow("", FontFactory.GetFont("Tahoma", 12f, 1), _nHeaderHeight, false);
                        }
                        else
                        {
                            PrintHeader();
                        }
                        PrintRow("Bank Payment Advice", FontFactory.GetFont("Tahoma", 12f, 1), 25, true);
                        if (nCount != _oEmployeeSalarys.Count)
                        {
                            _oPdfPCell = new PdfPCell(oHeaderPdfPTable);
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }
                        #endregion
                    }
                }
            }

            matchString = _selectedColNames.FirstOrDefault(x => x.Contains("Amount"));
            if (matchString != null)
            {
                #region Grand Total
                PdfPTable oPPdfPTable = new PdfPTable(_selectedColNames.Length + 1);
                oPPdfPTable.WidthPercentage = 100;
                oPPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oPPdfPTable.SetWidths(setWidth);

                _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold)); _oPdfPCell.Colspan = _selectedColNames.Length;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oEmployeeSalarys.Sum(x => x.BankAmount), 2).ToString("###,##0;(###,##0)"), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold)); _oPdfPCell.Colspan = _selectedColNames.Length + 1; _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("In Words : " + Global.TakaWords(Math.Round(_oEmployeeSalarys.Sum(x => x.BankAmount), 0)), _oFontStyleBold)); _oPdfPCell.Colspan = _selectedColNames.Length+1;
                _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPPdfPTable.AddCell(_oPdfPCell);

                oPPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(oPPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
        }
        private void PrintLastPart()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595 });
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Your co-operation in this regard would be highly appreciated.", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Thanking you", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintRow(string sText, iTextSharp.text.Font oStyle, float nHeight, bool bPrintUnder)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595 });

            #region Row
            if (bPrintUnder)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", oStyle)); _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(new Phrase(sText, oStyle)); _oPdfPCell.FixedHeight = nHeight; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        #region Support Functions
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 525f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
        #endregion
    }
}
