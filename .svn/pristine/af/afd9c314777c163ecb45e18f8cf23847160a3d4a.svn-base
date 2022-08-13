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
    public class rptCandidateAppplication
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        iTextSharp.text.Image _oImag;

        Candidate _oCandidate = new Candidate();
        List<Candidate> _oCandidates = new List<Candidate>();
        Company _oCompany = new Company();
        List<CandidateEducation> _oCandidateEducations = new List<CandidateEducation>();
        List<CandidateTraining> _oCandidateTrainings = new List<CandidateTraining>();
        List<CandidateExperience> _oCandidateExperiences = new List<CandidateExperience>();
        List<CandidateReference> _oCandidateReferences = new List<CandidateReference>();

        #endregion

        public byte[] PrepareReport(Candidate oCandidate)
        {
            _oCandidate = oCandidate;
            _oCandidateEducations = oCandidate.CandidateEducations;
            _oCandidateTrainings = oCandidate.CandidateTrainings;
            _oCandidateExperiences = oCandidate.CandidateExperiences;
            _oCandidateReferences = oCandidate.CandidateReferences;

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
            _oPdfPTable.SetWidths(new float[] { 100f, 200f,100, 150f });
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
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
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
            _oPdfPCell = new PdfPCell(new Phrase(_oCandidate.Name, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oCandidate.CandidatePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCandidate.CandidatePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
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

            _oPdfPCell = new PdfPCell(new Phrase(" " , _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Address : " + _oCandidate.PresentAddress, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Contact No : " + _oCandidate.ContactNo, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Email", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Email : " + _oCandidate.Email, _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

          
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" __________________________________________________________________________________________________________________", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

       
            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oCandidate.Objective != "")
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("Objective ", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(_oCandidate.Objective, _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" "));
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            if (_oCandidateExperiences.Count > 0)
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

                oPdfExperienceCell = new PdfPCell(new Phrase("Department", _oFontStyle));
                oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                oPdfExperienceCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
                oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                oPdfExperienceCell = new PdfPCell(new Phrase("Start Date", _oFontStyle));
                oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                oPdfExperienceCell = new PdfPCell(new Phrase("End Date", _oFontStyle));
                oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                oPdfExperienceTable.CompleteRow();

                int nExperienceCount = 0;
                foreach (CandidateExperience oCandidateExperience in _oCandidateExperiences)
                {
                    nExperienceCount++;
                    oPdfExperienceCell = new PdfPCell(new Phrase(nExperienceCount.ToString(), _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase(oCandidateExperience.Organization, _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase(oCandidateExperience.Department, _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase(oCandidateExperience.Designation, _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase(oCandidateExperience.StartDateInString, _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceCell = new PdfPCell(new Phrase(oCandidateExperience.EndDateInString, _oFontStyle));
                    oPdfExperienceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfExperienceCell.BackgroundColor = BaseColor.WHITE; oPdfExperienceTable.AddCell(oPdfExperienceCell);

                    oPdfExperienceTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfExperienceTable);
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
            if (_oCandidateEducations.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

                _oPdfPCell = new PdfPCell(new Phrase("Education", _oFontStyle)); _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                PdfPTable oPdfEducationTable = new PdfPTable(5);
                oPdfEducationTable.SetWidths(new float[] { 25f, 100f, 200, 100f, 100f });
                PdfPCell oPdfEducationCell;

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                oPdfEducationCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                oPdfEducationCell = new PdfPCell(new Phrase("Degree", _oFontStyle));
                oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                oPdfEducationCell = new PdfPCell(new Phrase("Board/University", _oFontStyle));
                oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                oPdfEducationCell = new PdfPCell(new Phrase("Passing Year", _oFontStyle));
                oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                oPdfEducationCell = new PdfPCell(new Phrase("Result", _oFontStyle));
                oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfEducationTable.AddCell(oPdfEducationCell);

                oPdfEducationTable.CompleteRow();

                int nEducationCount = 0;

                foreach (CandidateEducation oCandidateEducation in _oCandidateEducations)
                {
                    nEducationCount++;
                    oPdfEducationCell = new PdfPCell(new Phrase(nEducationCount.ToString(), _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase(oCandidateEducation.Degree, _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase(oCandidateEducation.BoardUniversity, _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase(oCandidateEducation.PassingYear.ToShortDateString(), _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationCell = new PdfPCell(new Phrase(oCandidateEducation.Result, _oFontStyle));
                    oPdfEducationCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfEducationCell.BackgroundColor = BaseColor.WHITE; oPdfEducationTable.AddCell(oPdfEducationCell);

                    oPdfEducationTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfEducationTable);
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

            if (_oCandidateTrainings.Count > 0)
            {
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

                oPdfTrainingCell = new PdfPCell(new Phrase("Institute", _oFontStyle));
                oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                oPdfTrainingCell = new PdfPCell(new Phrase("Start Date", _oFontStyle));
                oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                oPdfTrainingCell = new PdfPCell(new Phrase("End Date", _oFontStyle));
                oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                oPdfTrainingCell = new PdfPCell(new Phrase("Result", _oFontStyle));
                oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                oPdfTrainingTable.CompleteRow();

                int nTrainingCount = 0;
                foreach (CandidateTraining oCandidateTraining in _oCandidateTrainings)
                {
                    nTrainingCount++;
                    oPdfTrainingCell = new PdfPCell(new Phrase(nTrainingCount.ToString(), _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase(oCandidateTraining.CourseName, _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase(oCandidateTraining.Institution, _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase(oCandidateTraining.StartDateInString, _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase(oCandidateTraining.EndDateInString, _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingCell = new PdfPCell(new Phrase(oCandidateTraining.Result, _oFontStyle));
                    oPdfTrainingCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfTrainingCell.BackgroundColor = BaseColor.WHITE; oPdfTrainingTable.AddCell(oPdfTrainingCell);

                    oPdfTrainingTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfTrainingTable);
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
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.FatherName, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Mother's Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.MotherName, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Permanent Address", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.ParmanentAddress, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Date Of Birth", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.DateOfBirthInString, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Marital Status", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.MaritalStatus, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Religion", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.Religious, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Nationality", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.Nationalism, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("NationalID", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.ObjectID, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Present Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.PresentSalary, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Expected Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + _oCandidate.ExpectedSalary, _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            if (_oCandidateReferences.Count > 0)
            {
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

                oPdfReferenceCell = new PdfPCell(new Phrase("Department", _oFontStyle));
                oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                oPdfReferenceCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
                oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                oPdfReferenceCell = new PdfPCell(new Phrase("Contact No", _oFontStyle));
                oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                oPdfReferenceTable.CompleteRow();

                int nReferenceCount = 0;
                foreach (CandidateReference oCandidateReference in _oCandidateReferences)
                {
                    nReferenceCount++;
                    oPdfReferenceCell = new PdfPCell(new Phrase(nReferenceCount.ToString(), _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase(oCandidateReference.Name, _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase(oCandidateReference.Organization, _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase(oCandidateReference.Department, _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase(oCandidateReference.Designation, _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceCell = new PdfPCell(new Phrase(oCandidateReference.ContactNo, _oFontStyle));
                    oPdfReferenceCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfReferenceCell.BackgroundColor = BaseColor.WHITE; oPdfReferenceTable.AddCell(oPdfReferenceCell);

                    oPdfReferenceTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfReferenceTable);
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

            _oPdfPCell = new PdfPCell(new Phrase("____________________\nCandidate Signature", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



        }
        #endregion

    }

}
