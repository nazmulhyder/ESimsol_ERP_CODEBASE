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

    public class rptEmployeeJobApplicationInBangla
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(4);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        Employee _oEmployee = new Employee();

        #endregion

        public byte[] PrepareReport(Employee oEmployee)
        {

            _oEmployee = oEmployee;
            _oCompany = oEmployee.Company;

            #region Page Setup

            _oDocument = new Document(iTextSharp.text.PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 50f, 75f, 100f, 100f });

            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 10;
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
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Note, _oFontStyle));
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
            _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region applicationheader
            string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
            BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            _oFontStyle = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("ব্যবস্থাপনা পরিচালক" + "\n" + "Digicon Technologies Ltd. " + "\n" + "242, Tejgaon I/A, Gulshan Link Road, Dhaka -1208,", _oFontStyle));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("চাকুরীর আবেদন পত্র", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("তারিখ : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.FixedHeight = 8;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("বিষয় :  " + _oEmployee.DesignationName + " পদে চাকুরীর জন্য আবেদন ।", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("জনাব,", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("বিনীত নিবেদন এই যে, আপনার প্রকাশিত বিজ্ঞপ্তি/বিশ্বস্তসূত্রে জানতে পারলাম আপনার প্রতিষ্ঠানে " + _oEmployee.DesignationName + " পদে কিছু \n" + "সংখ্যক লোক নিয়োগ করা হবে । \n" +
             "আমি উক্ত পদে একজন প্রার্থী হিসেবে নিম্নে আমার জীবন বৃত্তান্ত পেশ করলাম। ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body

        private void PrintBody()
        {



            _oPdfPCell = new PdfPCell(new Phrase("১. নাম : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Name, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("২. পিতা/ স্বামীর নাম : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.FatherName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("৩. মাতার নাম : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.MotherName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("৪. বর্তামান ঠিকানা : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.PresentAddress, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(new Phrase("৫. স্থায়ী ঠিকানা : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("গ্রাম : " + _oEmployee.Village, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ডাকঘর : " + _oEmployee.PostOffice, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("উপজেলা : " + _oEmployee.Thana, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("জেলা : " + _oEmployee.District, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("৬. জন্ম তারিখ : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.DateOfBirthInString, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("৭. জন্ম নিবন্ধন নং : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.BirthID, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("৮. জাতীয় পরিচয় পত্র নং : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.NationalID,_oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("৯. বৈবাহিক অবস্থা : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.MaritalStatus, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("১০. সন্তান সংখ্যা(বিবাহিতদের ক্ষেত্রে) : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.ChildrenInfo, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

           
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("১১. ধর্ম : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Religious, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("১২. জাতীয়তা : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Nationalism, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("১৩. শিক্ষাগত যোগ্যতা : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Note, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("১৪. অভিজ্ঞতা  : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            PdfPTable _oPdfPTable1 = new PdfPTable(5);
            PdfPCell _oPdfPCell1;

            _oPdfPTable1.SetWidths(new float[] { 25f, 50f, 150f, 100f, 100f });

            _oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell1.Border = 0;
            _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

            _oPdfPCell1 = new PdfPCell(new Phrase("ক্রঃ নং ", _oFontStyle));
            _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

            _oPdfPCell1 = new PdfPCell(new Phrase("কারখানার নাম", _oFontStyle));
            _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

            _oPdfPCell1 = new PdfPCell(new Phrase("পদবি", _oFontStyle));
            _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

            _oPdfPCell1 = new PdfPCell(new Phrase("চাকুরীর কাল", _oFontStyle));
            _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

            _oPdfPTable1.CompleteRow();

            int nCount = 0;
            foreach (EmployeeExperience oItem in _oEmployee.EmployeeExperiences)
            {
                nCount++;

                _oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell1.Border = 0;
                _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);


                _oPdfPCell1 = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

                _oPdfPCell1 = new PdfPCell(new Phrase(oItem.Organization, _oFontStyle));
                _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

                _oPdfPCell1 = new PdfPCell(new Phrase(oItem.Designation, _oFontStyle));
                _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);

                _oPdfPCell1 = new PdfPCell(new Phrase(oItem.DurationString, _oFontStyle));
                _oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell1.BackgroundColor = BaseColor.WHITE; _oPdfPTable1.AddCell(_oPdfPCell1);


                _oPdfPTable1.CompleteRow();



            }


            _oPdfPCell = new PdfPCell(_oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("উল্লেখ্য যে আমি কর্তৃপক্ষের আদেশ ক্রমে যে কোন গেজ মেশিন অপারেটর হিসাবে কাজ করতে বাধ্য থাকিব। " + "জনাবের নিকট প্রার্থনা আমার আবেদন সু-বিবেচনা করতঃ আমাকে উক্ত পদে নিয়োগ দানে মর্জি হয় । আমি নিম্নসাক্ষরকারী সজ্ঞানে সকল তথ্য প্রদান "
             + "করলাম। ভবিষ্যতে আমার প্রদানকৃত তথ্য মিথ্যা বলে প্রতীয়মান হলে কারখানা কর্তৃপক্ষ আমার বিরুদ্দে আইনগত ব্যবস্থা নিতে পারিবেন।", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("সংযুক্ত দলিলাদি :- ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("১. জাতীয় পরিচয় পত্র/ জন্ম সনদ  ।", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("২. চেয়ারম্যান সার্টিফিকেট । ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("৩. পাসপোর্ট সাইজ ছবি(৩ কপি). ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("নিবেদক/ নিবেদিকা  ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("৪. স্ট্যাম্প সাইজ ছবি(১ কপি)  ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.NameInBangla, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("৫. শিক্ষাগত যোগ্যতার প্রমান পত্র ।", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("তারিখ : ", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

        }
        #endregion
    }

}
