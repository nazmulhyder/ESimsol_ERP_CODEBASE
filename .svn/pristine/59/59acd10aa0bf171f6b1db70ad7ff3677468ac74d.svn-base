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
    public class rptCrewInformation
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        iTextSharp.text.Image _oImag;
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        Company _oCompany = new Company();
        EmployeeBankAccount _oEmployeeBankAccount = new EmployeeBankAccount();
        EmployeeReference _oEmployeeReference = new EmployeeReference();

        #endregion

        public byte[] PrepareReport(Employee oEmployee)
        {
            _oEmployee = oEmployee;
            _oCompany = oEmployee.Company;
            if (oEmployee.EmployeeBankAccounts.Count > 0)
            {
                _oEmployeeBankAccount = oEmployee.EmployeeBankAccounts.First();
            }
            if (oEmployee.EmployeeReferences.Count > 0)
            {
                _oEmployeeReference = oEmployee.EmployeeReferences.First();
            }

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(1000, 500), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 200f, 200f,200f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(3);
            oPdfPTableHeader.SetWidths(new float[] { 170f, 40f, 390f });
            PdfPCell oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 20;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(20f, 15f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 20;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = 6;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 20;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 20;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
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
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Applying For Position Of ", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.DesignationName));
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
           

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body

        private void PrintBody()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("A. PARTICULARS OF APPLICANT"));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PdfPTable oPdfPCrewInformationTable = new PdfPTable(8);
            oPdfPCrewInformationTable.SetWidths(new float[] { 100f, 60f, 100f, 60f, 100f, 60f, 100f, 70f });
            PdfPCell oPdfPCellCrewInformation;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Name : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.NickName, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Last Name : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.Name, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("SKMS Crew ID No : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("C rew IPN", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCrewInformationTable.CompleteRow();

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Address : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.ParmanentAddress, _oFontStyle)); oPdfPCellCrewInformation.Colspan = 7;
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCrewInformationTable.CompleteRow();

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Phone No. : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.ContactNo, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Height(cm) : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.Height, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Blood Group : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.BloodGroup, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Weight(kg)", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.Weight, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCrewInformationTable.CompleteRow();

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Seatime (Present Rank) : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.DesignationName, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Nationality : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.Nationalism, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Waist (cm) : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.Waist, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Shoe Size : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.ShoeSize, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCrewInformationTable.CompleteRow();

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Next of Kin : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployeeReference.Name, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);


            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Relationship : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);


            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployeeReference.Relation, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);


            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Place of Birth : ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.BirthPlace, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase("Date of Birth: ", _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCellCrewInformation = new PdfPCell(new Phrase(_oEmployee.DateOfBirthInString, _oFontStyle));
            oPdfPCellCrewInformation.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCrewInformation.BackgroundColor = BaseColor.WHITE; oPdfPCrewInformationTable.AddCell(oPdfPCellCrewInformation);

            oPdfPCrewInformationTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPCrewInformationTable);
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oEmployee.EmployeeBankAccounts.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("B. BANK DETAILS"));
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPBankTable = new PdfPTable(8);
                oPdfPBankTable.SetWidths(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
                PdfPCell oPdfPCellBank;

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellBank = new PdfPCell(new Phrase("Bank Name : ", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase(_oEmployeeBankAccount.BankBranchName, _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Account Name : ", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase(_oEmployeeBankAccount.AccountName, _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Pass port :", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase(_oEmployee.PassportNo, _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Seaman Book", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPBankTable.CompleteRow();

                oPdfPCellBank = new PdfPCell(new Phrase("Bank Address : ", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase(_oEmployeeBankAccount.BankAddress, _oFontStyle)); oPdfPCellBank.Colspan = 3;
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Issued : ", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Issued", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPBankTable.CompleteRow();

                oPdfPCellBank = new PdfPCell(new Phrase("Bank Account No : ", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase(_oEmployeeBankAccount.AccountNo, _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Swift Code : ", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase(_oEmployeeBankAccount.SwiftCode, _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Expiry:", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("Expiry:", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPCellBank = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCellBank.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellBank.BackgroundColor = BaseColor.WHITE; oPdfPBankTable.AddCell(oPdfPCellBank);

                oPdfPBankTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPBankTable);
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oEmployee.EmployeeTrainings.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("C. PARTICULARS OF OFFICIAL DOCUMENTS"));
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPCertificateTable = new PdfPTable(8);
                oPdfPCertificateTable.SetWidths(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
                PdfPCell oPdfPCellCertificate;

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellCertificate = new PdfPCell(new Phrase("SL No.", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Certificate/Documents  ", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Req'd For", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Grade", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Document No", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Issued", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Expiry", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCellCertificate = new PdfPCell(new Phrase("Institute Attended/Issued By", _oFontStyle));
                oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                oPdfPCertificateTable.CompleteRow();

                int nEducationNo = 0;
                foreach (EmployeeTraining oETraining in _oEmployee.EmployeeTrainings)
                {
                    nEducationNo++;
                    oPdfPCellCertificate = new PdfPCell(new Phrase(nEducationNo.ToString(), _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.CourseName, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.RequiredFor, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.Result, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.CertificateNo, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.StartDateInString, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.EndDateInString, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCellCertificate = new PdfPCell(new Phrase(oETraining.Institution, _oFontStyle));
                    oPdfPCellCertificate.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellCertificate.BackgroundColor = BaseColor.WHITE; oPdfPCertificateTable.AddCell(oPdfPCellCertificate);

                    oPdfPCertificateTable.CompleteRow();

                }
                _oPdfPCell = new PdfPCell(oPdfPCertificateTable);
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oEmployee.EmployeeExperiences.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("D. PARTICULARS OF SEA SERVICE"));
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPExperienceTable = new PdfPTable(11);
                oPdfPExperienceTable.SetWidths(new float[] { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
                PdfPCell oPdfPCellExperience;

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCellExperience = new PdfPCell(new Phrase("SL No  ", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("COMPANY ", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("RANK ", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("PERIOD ", _oFontStyle)); oPdfPCellExperience.Colspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("VLS TYPE ", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("VESSEL NAME", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("GRT", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("DWT", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("ENGINE", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("BHP", _oFontStyle)); oPdfPCellExperience.Rowspan = 2;
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);


                oPdfPExperienceTable.CompleteRow();

                oPdfPCellExperience = new PdfPCell(new Phrase("S. ON", _oFontStyle));
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPCellExperience = new PdfPCell(new Phrase("S. OFF", _oFontStyle));
                oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                oPdfPExperienceTable.CompleteRow();

                int nExperienceNo = 0;
                foreach (EmployeeExperience oEx in _oEmployee.EmployeeExperiences)
                {
                    nExperienceNo++;
                    oPdfPCellExperience = new PdfPCell(new Phrase(nExperienceNo.ToString(), _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.ContractorName, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.DesignationName, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.StartDateString, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.EndDateString, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.VesselTypeInString, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.VesselName, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.GRT, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.DWT, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.EngineType, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPCellExperience = new PdfPCell(new Phrase(oEx.BHP, _oFontStyle));
                    oPdfPCellExperience.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperience.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPCellExperience);

                    oPdfPExperienceTable.CompleteRow();

                }
                _oPdfPCell = new PdfPCell(oPdfPExperienceTable);
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }

        }
        #endregion

    }

}
