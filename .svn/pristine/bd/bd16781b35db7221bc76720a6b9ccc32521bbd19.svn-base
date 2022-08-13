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

    public class rptEmployeeSelfInformation
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Employee _oEmployee = new Employee();
        Company _oCompany = new Company();
        List<EmployeeEducation> _oEmployeeEducations = new List<EmployeeEducation>();
        List<EmployeeExperience> _oEmployeeExperiences = new List<EmployeeExperience>();
        List<EmployeeTraining> _oEmployeeTrainings = new List<EmployeeTraining>();
        List<EmployeeReference> _oEmployeeReferences = new List<EmployeeReference>();
        List<EmployeeBankAccount> _oEmployeeBankAccounts = new List<EmployeeBankAccount>();
        List<EmployeeAuthentication> _oEmployeeAuthentications = new List<EmployeeAuthentication>();

        #endregion

        public byte[] PrepareReport(Employee oEmployee)
        {
            _oEmployee = oEmployee;
            _oEmployeeEducations = oEmployee.EmployeeEducations;
            _oEmployeeExperiences = oEmployee.EmployeeExperiences;
            _oEmployeeTrainings = oEmployee.EmployeeTrainings;
            _oEmployeeReferences = oEmployee.EmployeeReferences;
            _oEmployeeBankAccounts = oEmployee.EmployeeBankAccounts;
            _oEmployeeAuthentications = oEmployee.EmployeeAuthentications;
            _oCompany = oEmployee.Company;

            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f,100f });

            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 6;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 160f, 255f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(20f, 15f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                //oPdfPCellHearder.PaddingBottom = 8;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.Colspan = 3; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));

            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
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


            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" IMFORMATION OF "+_oEmployee.Name, _oFontStyle));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 4;
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


            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
            _oFontStyle.IsUnderlined();
            _oPdfPCell = new PdfPCell(new Phrase("Basic Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("Name : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Name, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Nick Name : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.NickName, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Contact No. : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.ContactNo, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Email : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Email, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Present Address : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.PresentAddress, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Permanent Address : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.ParmanentAddress, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
            _oFontStyle.IsUnderlined();
            _oPdfPCell = new PdfPCell(new Phrase("Official Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("Gender : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Gender, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Marital Status : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.MaritalStatus, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Father's Nmae : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.FatherName, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Mather's Name : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.MotherName, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Date Of Birth : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.DateOfBirthInString, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Identification Mark : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.IdentificationMart, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Blood Gruop : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.BloodGroup, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Weight : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Weight, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Height : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Height, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Note : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Note, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Designation Type : ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.EmployeeTypeName, _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            if (_oEmployeeEducations.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
                _oFontStyle.IsUnderlined();
                _oPdfPCell = new PdfPCell(new Phrase("Educational Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPEducationTable = new PdfPTable(6);
                oPdfPEducationTable.SetWidths(new float[] { 30f, 70f, 70f, 70f, 100f, 100f });
                PdfPCell oPdfPEducationCell;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);


                oPdfPEducationCell = new PdfPCell(new Phrase("Sl No : ", _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                oPdfPEducationCell = new PdfPCell(new Phrase("Degree : ", _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                oPdfPEducationCell = new PdfPCell(new Phrase("Major : ", _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                oPdfPEducationCell = new PdfPCell(new Phrase("Session : ", _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                oPdfPEducationCell = new PdfPCell(new Phrase("Pssing Year : ", _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                oPdfPEducationCell = new PdfPCell(new Phrase("Board/University : ", _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                oPdfPEducationTable.CompleteRow();

                int nEducationCount = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                foreach (EmployeeEducation oEmployeeEducation in _oEmployeeEducations)
                {

                    nEducationCount++;

                    oPdfPEducationCell = new PdfPCell(new Phrase(nEducationCount.ToString(), _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                    oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                    oPdfPEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.Degree, _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                    oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                    oPdfPEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.Major, _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                    oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                    oPdfPEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.Session, _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                    oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                    oPdfPEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.PassingYear.ToString(), _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                    oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                    oPdfPEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.BoardUniversity, _oFontStyle)); oPdfPEducationCell.FixedHeight = 15;
                    oPdfPEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPEducationCell.BackgroundColor = BaseColor.WHITE; oPdfPEducationTable.AddCell(oPdfPEducationCell);

                    oPdfPEducationTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPEducationTable);
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


            if (_oEmployeeExperiences.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
                _oFontStyle.IsUnderlined();
                _oPdfPCell = new PdfPCell(new Phrase("Experience Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPExperienceTable = new PdfPTable(5);
                oPdfPExperienceTable.SetWidths(new float[] { 30f, 70f, 70f, 70f, 100f});
                PdfPCell oPdfPExperienceCell;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);


                oPdfPExperienceCell = new PdfPCell(new Phrase("Sl No : ", _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                oPdfPExperienceCell = new PdfPCell(new Phrase("Organization : ", _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                oPdfPExperienceCell = new PdfPCell(new Phrase("Type : ", _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                oPdfPExperienceCell = new PdfPCell(new Phrase("Designation : ", _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                oPdfPExperienceCell = new PdfPCell(new Phrase("Duration : ", _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                oPdfPExperienceTable.CompleteRow();

                int nExparienceCount = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                foreach (EmployeeExperience oEmployeeExperience in _oEmployeeExperiences)
                {

                    nExparienceCount++;

                    oPdfPExperienceCell = new PdfPCell(new Phrase(nExparienceCount.ToString(), _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                    oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                    oPdfPExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.Organization, _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                    oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                    oPdfPExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.OrganizationType, _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                    oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                    oPdfPExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.Designation, _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                    oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                    oPdfPExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.DurationString, _oFontStyle)); oPdfPExperienceCell.FixedHeight = 15;
                    oPdfPExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfPExperienceTable.AddCell(oPdfPExperienceCell);

                    oPdfPExperienceTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPExperienceTable);
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            if (_oEmployeeTrainings.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
                _oFontStyle.IsUnderlined();
                _oPdfPCell = new PdfPCell(new Phrase("Training Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPTrainingTable = new PdfPTable(6);
                oPdfPTrainingTable.SetWidths(new float[] { 30f, 70f, 70f, 70f, 100f,100f });
                PdfPCell oPdfPTrainingCell;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);


                oPdfPTrainingCell = new PdfPCell(new Phrase("Sl No : ", _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                oPdfPTrainingCell = new PdfPCell(new Phrase("Course Name : ", _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                oPdfPTrainingCell = new PdfPCell(new Phrase("Specification : ", _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                oPdfPTrainingCell = new PdfPCell(new Phrase("Duration : ", _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                oPdfPTrainingCell = new PdfPCell(new Phrase("Passing Year : ", _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                oPdfPTrainingCell = new PdfPCell(new Phrase("Certify Body Vendor : ", _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                oPdfPTrainingTable.CompleteRow();

                int nTrainingCount = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                foreach (EmployeeTraining oEmployeeTraining in _oEmployeeTrainings)
                {

                    nTrainingCount++;

                    oPdfPTrainingCell = new PdfPCell(new Phrase(nTrainingCount.ToString(), _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                    oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                    oPdfPTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.CourseName, _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                    oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                    oPdfPTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.Specification, _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                    oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                    oPdfPTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.DurationString, _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                    oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                    oPdfPTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.PassingDateInString, _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                    oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                    oPdfPTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.CertifyBodyVendor, _oFontStyle)); oPdfPTrainingCell.FixedHeight = 15;
                    oPdfPTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfPTrainingTable.AddCell(oPdfPTrainingCell);

                    oPdfPTrainingTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPTrainingTable);
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            if (_oEmployeeReferences.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
                _oFontStyle.IsUnderlined();
                _oPdfPCell = new PdfPCell(new Phrase("Reference Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPReferenceTable = new PdfPTable(6);
                oPdfPReferenceTable.SetWidths(new float[] { 30f, 70f, 70f, 70f, 100f, 100f });
                PdfPCell oPdfPReferenceCell;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);


                oPdfPReferenceCell = new PdfPCell(new Phrase("Sl No : ", _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                oPdfPReferenceCell = new PdfPCell(new Phrase("Name : ", _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                oPdfPReferenceCell = new PdfPCell(new Phrase("Organization : ", _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                oPdfPReferenceCell = new PdfPCell(new Phrase("Designstion : ", _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                oPdfPReferenceCell = new PdfPCell(new Phrase("Relation : ", _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                oPdfPReferenceCell = new PdfPCell(new Phrase("Contact No : ", _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                oPdfPReferenceTable.CompleteRow();

                int nReferenceCount = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                foreach (EmployeeReference oEmployeeReference in _oEmployeeReferences)
                {

                    nReferenceCount++;

                    oPdfPReferenceCell = new PdfPCell(new Phrase(nReferenceCount.ToString(), _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                    oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                    oPdfPReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Name, _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                    oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                    oPdfPReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Organization, _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                    oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                    oPdfPReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Designation, _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                    oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                    oPdfPReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Relation, _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                    oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                    oPdfPReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.ContactNo, _oFontStyle)); oPdfPReferenceCell.FixedHeight = 15;
                    oPdfPReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfPReferenceTable.AddCell(oPdfPReferenceCell);

                    oPdfPReferenceTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPReferenceTable);
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            if (_oEmployeeBankAccounts.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
                _oFontStyle.IsUnderlined();
                _oPdfPCell = new PdfPCell(new Phrase("Reference Information ", _oFontStyle)); _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfPBankAccountTable = new PdfPTable(6);
                oPdfPBankAccountTable.SetWidths(new float[] { 30f, 70f, 70f, 70f, 100f, 100f });
                PdfPCell oPdfPBankAccountCell;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);


                oPdfPBankAccountCell = new PdfPCell(new Phrase("Sl No : ", _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                oPdfPBankAccountCell = new PdfPCell(new Phrase("Bank Brance Name : ", _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                oPdfPBankAccountCell = new PdfPCell(new Phrase("Account Name : ", _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                oPdfPBankAccountCell = new PdfPCell(new Phrase("Account No. : ", _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                oPdfPBankAccountCell = new PdfPCell(new Phrase("Activity : ", _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                oPdfPBankAccountCell = new PdfPCell(new Phrase("Description : ", _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                oPdfPBankAccountTable.CompleteRow();

                int nBankAccountCount = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                foreach (EmployeeBankAccount oEmployeeBankAccount in _oEmployeeBankAccounts)
                {

                    nBankAccountCount++;

                    oPdfPBankAccountCell = new PdfPCell(new Phrase(nBankAccountCount.ToString(), _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                    oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                    oPdfPBankAccountCell = new PdfPCell(new Phrase(oEmployeeBankAccount.BankBranchName, _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                    oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                    oPdfPBankAccountCell = new PdfPCell(new Phrase(oEmployeeBankAccount.AccountName, _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                    oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                    oPdfPBankAccountCell = new PdfPCell(new Phrase(oEmployeeBankAccount.AccountNo, _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                    oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                    oPdfPBankAccountCell = new PdfPCell(new Phrase(oEmployeeBankAccount.Activity, _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                    oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                    oPdfPBankAccountCell = new PdfPCell(new Phrase(oEmployeeBankAccount.Description, _oFontStyle)); oPdfPBankAccountCell.FixedHeight = 15;
                    oPdfPBankAccountCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPBankAccountCell.BackgroundColor = BaseColor.WHITE; oPdfPBankAccountTable.AddCell(oPdfPBankAccountCell);

                    oPdfPBankAccountTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPBankAccountTable);
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

        }
        #endregion
    }




}
