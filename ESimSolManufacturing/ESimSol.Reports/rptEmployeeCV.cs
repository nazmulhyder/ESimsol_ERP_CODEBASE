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
    public class rptEmployeeCV
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        iTextSharp.text.Image _oImag;

        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        Company _oCompany = new Company();
        List<EmployeeEducation> _oEmployeeEducations = new List<EmployeeEducation>();
        List<EmployeeTraining> _oEmployeeTrainings = new List<EmployeeTraining>();
        List<EmployeeExperience> _oEmployeeExperiences = new List<EmployeeExperience>();
        List<EmployeeReference> _oEmployeeReferences = new List<EmployeeReference>();
        List<EmployeeBankAccount> _oEmployeeBankAccounts = new List<EmployeeBankAccount>();
        EmployeeOfficial _oEmployeeOfficial = new EmployeeOfficial();
        AttendanceScheme _oAttendanceScheme = new AttendanceScheme();
        List<AttendanceSchemeHoliday> _oAttendanceSchemeHolidays = new List<AttendanceSchemeHoliday>();
        List<AttendanceSchemeLeave> _oAttendanceSchemeLeaves = new List<AttendanceSchemeLeave>();
        List<RosterPlanDetail> _oRosterPlanDetails = new List<RosterPlanDetail>();
        EmployeeSalaryStructure _oEmployeeSalaryStructure = new EmployeeSalaryStructure();
        List<EmployeeSalaryStructureDetail> _oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();

        bool _IsBasicInfo = false;
        bool _IsOfficialInfo = false;
        bool _IsSalaryInfo = false;
        #endregion

        public byte[] PrepareReport(Employee oEmployee)
        {
            _oEmployee = oEmployee;
            _oEmployeeEducations = oEmployee.EmployeeEducations;
            _oEmployeeTrainings = oEmployee.EmployeeTrainings;
            _oEmployeeExperiences = oEmployee.EmployeeExperiences;
            _oEmployeeReferences = oEmployee.EmployeeReferences;
            _oEmployeeBankAccounts = oEmployee.EmployeeBankAccounts;
            _oEmployeeOfficial = oEmployee.EmployeeOfficial;
            _oAttendanceScheme = oEmployee.AttendanceScheme;
            _oAttendanceSchemeHolidays = oEmployee.AttendanceSchemeHolidays;
            _oAttendanceSchemeLeaves = oEmployee.AttendanceSchemeLeaves;
            _oRosterPlanDetails = oEmployee.RosterPlanDetails;
            _oEmployeeSalaryStructure = oEmployee.EmployeeSalaryStructure;
            _oEmployeeSalaryStructureDetails = oEmployee.EmployeeSalaryStructureDetails;
            _oCompany = oEmployee.Company;


            _IsBasicInfo = Convert.ToBoolean(oEmployee.ErrorMessage.Split('~')[1]);
            _IsOfficialInfo = Convert.ToBoolean(oEmployee.ErrorMessage.Split('~')[2]);
            _IsSalaryInfo = Convert.ToBoolean(oEmployee.ErrorMessage.Split('~')[3]);

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
            _oPdfPTable.SetWidths(new float[] { 100f, 200f, 100, 150f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {

            //_oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.UNDERLINE);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 4;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 30;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 75f, 120f });
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
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
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
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Name, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oEmployee.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oEmployee.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 65f);
                _oPdfPCell = new PdfPCell(_oImag);
                //_oPdfPCell.FixedHeight = 65;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.Rowspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.PaddingBottom = 10;
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);

            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Rowspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Contact No", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Department : " + _oEmployee.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            _oPdfPCell = new PdfPCell(new Phrase("Designation : " + _oEmployee.DesignationName, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Contact No : " + _oEmployee.ContactNo, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //_oPdfPCell = new PdfPCell(new Phrase("Email : " + _oEmployee.Email, _oFontStyle)); _oPdfPCell.Colspan = 2;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" __________________________________________________________________________________________________________________", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_IsBasicInfo == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Basic Info", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                //if (_oEmployee.Objective != "")
                //{
                //    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                //    _oPdfPCell = new PdfPCell(new Phrase("Objective ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                //    _oPdfPTable.CompleteRow();

                //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                //    _oPdfPTable.CompleteRow();

                //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //    _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Objective, _oFontStyle)); _oPdfPCell.Colspan = 4;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //    _oPdfPTable.CompleteRow();

                //    _oPdfPCell = new PdfPCell(new Phrase(" "));
                //    _oPdfPCell.Colspan = 4;
                //    _oPdfPCell.Border = 0;
                //    _oPdfPCell.FixedHeight = 20;
                //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //    _oPdfPTable.AddCell(_oPdfPCell);
                //    _oPdfPTable.CompleteRow();
                //}

                if (_oEmployeeExperiences.Count > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("Experience", _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    PdfPTable oPdfExperienceTable = new PdfPTable(6);
                    oPdfExperienceTable.SetWidths(new float[] { 25f, 100f, 100, 100f, 100f, 100f });
                    PdfPCell oPdfExperienceCell;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    oPdfExperienceCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase("Organization", _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase("Organization Type", _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase("Start Date", _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase("End Date", _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceTable.CompleteRow();

                    int nExperienceCount = 0;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    foreach (EmployeeExperience oEmployeeExperience in _oEmployeeExperiences)
                    {
                        nExperienceCount++;
                        oPdfExperienceCell = new PdfPCell(new Phrase(nExperienceCount.ToString(), _oFontStyle));
                        oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                        oPdfExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.Organization, _oFontStyle));
                        oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                        oPdfExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.OrganizationType, _oFontStyle));
                        oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                        oPdfExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.Designation, _oFontStyle));
                        oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                        oPdfExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.StartDateString, _oFontStyle));
                        oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                        oPdfExperienceCell = new PdfPCell(new Phrase(oEmployeeExperience.EndDateString, _oFontStyle));
                        oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                        oPdfExperienceTable.CompleteRow();
                    }

                    _oPdfPCell = new PdfPCell(oPdfExperienceTable);
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                  
                }
                if (_oEmployeeEducations.Count > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" "));
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("Education", _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    PdfPTable oPdfEducationTable = new PdfPTable(6);
                    oPdfEducationTable.SetWidths(new float[] { 25f, 100f, 150f, 100f, 100f,100f });
                    PdfPCell oPdfEducationCell;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    oPdfEducationCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase("Degree", _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase("Session", _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase("Major", _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase("Passing Year", _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase("Board/University", _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                   
                    oPdfEducationTable.CompleteRow();

                    int nEducationCount = 0;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    foreach (EmployeeEducation oEmployeeEducation in _oEmployeeEducations)
                    {
                        nEducationCount++;
                        oPdfEducationCell = new PdfPCell(new Phrase(nEducationCount.ToString(), _oFontStyle));
                        oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                        oPdfEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.Degree, _oFontStyle));
                        oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                        oPdfEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.Session, _oFontStyle));
                        oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                        oPdfEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.Major, _oFontStyle));
                        oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                        oPdfEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.PassingYear.ToString(), _oFontStyle));
                        oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                        oPdfEducationCell = new PdfPCell(new Phrase(oEmployeeEducation.BoardUniversity, _oFontStyle));
                        oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                        oPdfEducationTable.CompleteRow();
                    }

                    _oPdfPCell = new PdfPCell(oPdfEducationTable);
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                  
                }

                if (_oEmployeeTrainings.Count > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" "));
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("Training", _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    PdfPTable oPdfTrainingTable = new PdfPTable(6);
                    oPdfTrainingTable.SetWidths(new float[] { 25f, 100f, 100, 100f, 100f, 100f });
                    PdfPCell oPdfTrainingCell;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    oPdfTrainingCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase("Coursename", _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase("Sepecification", _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase("Duration", _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase("Passing Year", _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase("Certify B. Vendor", _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingTable.CompleteRow();

                    int nTrainingCount = 0;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    foreach (EmployeeTraining oEmployeeTraining in _oEmployeeTrainings)
                    {
                        nTrainingCount++;
                        oPdfTrainingCell = new PdfPCell(new Phrase(nTrainingCount.ToString(), _oFontStyle));
                        oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                        oPdfTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.CourseName, _oFontStyle));
                        oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                        oPdfTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.Specification, _oFontStyle));
                        oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                        oPdfTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.DurationString, _oFontStyle));
                        oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                        oPdfTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.PassingDateInString, _oFontStyle));
                        oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                        oPdfTrainingCell = new PdfPCell(new Phrase(oEmployeeTraining.CertifyBodyVendor, _oFontStyle));
                        oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                        oPdfTrainingTable.CompleteRow();
                    }

                    _oPdfPCell = new PdfPCell(oPdfTrainingTable);
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                  
                }

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Personal Details ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Father's Name", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.FatherName, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Mother's Name", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.MotherName, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Present Address", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.PresentAddress, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Permanent Address", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.ParmanentAddress, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Email", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.Email, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Date Of Birth", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.DateOfBirthInString, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Marital Status", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.MaritalStatus, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Religion", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.Religious, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Nationality", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.Nationalism, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("NationalID", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" : " + _oEmployee.ObjectID, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                if (_oEmployeeReferences.Count > 0)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(" "));
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("Reference", _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    PdfPTable oPdfReferenceTable = new PdfPTable(6);
                    oPdfReferenceTable.SetWidths(new float[] { 25f, 100f, 100, 100f, 100f, 100f });
                    PdfPCell oPdfReferenceCell;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    oPdfReferenceCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase("Name", _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase("Organization", _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);
                   
                    oPdfReferenceCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);
                    
                    oPdfReferenceCell = new PdfPCell(new Phrase("Relation", _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase("Contact No", _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceTable.CompleteRow();

                    int nReferenceCount = 0;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    foreach (EmployeeReference oEmployeeReference in _oEmployeeReferences)
                    {
                        nReferenceCount++;
                        oPdfReferenceCell = new PdfPCell(new Phrase(nReferenceCount.ToString(), _oFontStyle));
                        oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                        oPdfReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Name, _oFontStyle));
                        oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                        oPdfReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Organization, _oFontStyle));
                        oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                        oPdfReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Designation, _oFontStyle));
                        oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                        oPdfReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.Relation, _oFontStyle));
                        oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                        oPdfReferenceCell = new PdfPCell(new Phrase(oEmployeeReference.ContactNo, _oFontStyle));
                        oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                        oPdfReferenceTable.CompleteRow();
                    }

                    _oPdfPCell = new PdfPCell(oPdfReferenceTable);
                    _oPdfPCell.Colspan = 4;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();


                }
            }
            if (_IsOfficialInfo == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Official Info", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


                _oPdfPCell = new PdfPCell(new Phrase("Location: " , _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeOfficial.LocationName, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);


                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Shift: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeOfficial.CurrentShiftName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Dept. Policy Name: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeOfficial.DRPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Attendance Calender  : ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oAttendanceScheme.AttendanceCalendar, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Day Off: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oAttendanceScheme.DayOff, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Employee Type: " , _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeOfficial.EmployeeTypeName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Date Of Join: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeOfficial.DateOfJoinInString, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Date Of Confirmation : " , _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeOfficial.DateOfConfirmationInString , _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Roster Plan - " + _oAttendanceScheme.RosterPlanDescription, _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Shift ", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;  _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Next Shift", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;  _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (RosterPlanDetail oItem in _oRosterPlanDetails)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Shift, _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NextShift, _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Leave" , _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Leave Name", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total Day", _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Deferred Day", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Activation After", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (AttendanceSchemeLeave oItem in _oAttendanceSchemeLeaves)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LeaveName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.TotalDay.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeferredDay.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ActivationAfterInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                string sHDays = "";
                foreach(AttendanceSchemeHoliday oItem in _oAttendanceSchemeHolidays)
                {
                    sHDays = sHDays + oItem.HoliDayName + ",";
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Holiday Days: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(sHDays, _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 10;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                if (_oAttendanceScheme.OverTime == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Overtime Activation After " + _oAttendanceScheme.OverTimeDeferredDay + " Days Of " + _oAttendanceScheme.OverTimeActivationAfterInString, _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                //if (_oAttendanceScheme.AlternativeDayOff == true)
                //{
                //    _oPdfPCell = new PdfPCell(new Phrase("Alternative Day Off Activation After " + _oAttendanceScheme.AlternativeDayOffDeferredDay + " Days Of " + _oAttendanceScheme.AlternativeDayOffActivationAfterInString, _oFontStyle)); _oPdfPCell.Colspan = 4;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                //    _oPdfPTable.CompleteRow();
                //}
                if (_oAttendanceScheme.Accomodation == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Accomodation Activation After " + _oAttendanceScheme.AccommodationDeferredDay + " Days Of " + _oAttendanceScheme.AccommodationActivationAfterInString, _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                if (_oAttendanceScheme.Accomodation == true)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Accomodation Activation After " + _oAttendanceScheme.AccommodationDeferredDay + " Days Of " + _oAttendanceScheme.AccommodationActivationAfterInString, _oFontStyle)); _oPdfPCell.Colspan = 4;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }

            }
            if (_IsSalaryInfo == true)
            {
                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 10;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Salary Info", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Salary Scheme: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeSalaryStructure.SalarySchemeName, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Payment Cycle: " , _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeSalaryStructure.PaymentCycleInString, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Gross Salary: ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeSalaryStructure.CurrencySymbol + " " + Global.MillionFormat(_oEmployeeSalaryStructure.GrossAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Salary Detail", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Head Name", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Equation", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach(EmployeeSalaryStructureDetail oItem in _oEmployeeSalaryStructureDetails)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Calculation, _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeSalaryStructure.CurrencySymbol +" "+ Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }

            }

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("____________________\nEmployee Signature", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



        }
        #endregion

    }

}
