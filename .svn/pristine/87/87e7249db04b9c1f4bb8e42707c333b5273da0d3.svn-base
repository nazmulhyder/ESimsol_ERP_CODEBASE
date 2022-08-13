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
    public class rptEmployeeCard
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable=new PdfPTable(3);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        Company _oCompany = new Company();
        DateTime _dIssueDate=DateTime.Today;
        DateTime _dExpireDate = DateTime.Today;
        #endregion

        public byte[] PrepareReport(Employee oEmployee, DateTime dIssueDate, DateTime dExpireDate)
        {
            _oEmployees = oEmployee.EmployeeHrms;
            _dIssueDate = dIssueDate;
            _dExpireDate = dExpireDate;
            _oCompany = oEmployee.Company;
            
            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 5f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 272f, 16f, 272f});
            #endregion

            this.PrintHeader();
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
           
            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 4;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            for (int i = 0; i < _oEmployees.Count; i=i+2)
            {
                _oPdfPCell = new PdfPCell(GetEmployeeCard(_oEmployees[i])); _oPdfPCell.Padding = 5;
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

               _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; 
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               
               if (i + 1 < _oEmployees.Count)
               {
                   _oPdfPCell = new PdfPCell(GetEmployeeCard(_oEmployees[i + 1])); _oPdfPCell.Padding = 5;
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               }
               else
               {
                   _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0;
                   _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               }
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
        }

        public PdfPTable GetEmployeeCard(Employee oEmployee)
        {

            PdfPTable oPdfPTable2 = new PdfPTable(4);
            PdfPCell oPdfPCell2;

            oPdfPTable2.SetWidths(new float[] { 55f,100f, 55f,70f});

            PdfPTable oPdfPTable3 = new PdfPTable(3);
            PdfPCell oPdfPCell3;

            oPdfPTable3.SetWidths(new float[] { 30f, 190f, 60f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(28f, 20f);
                oPdfPCell3 = new PdfPCell(_oImag);
                oPdfPCell3.Rowspan = 2;
                oPdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell3.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell3.PaddingBottom = 8;
                oPdfPCell3.Border = 0;

                oPdfPTable3.AddCell(oPdfPCell3);

            }
            else
            {
                oPdfPCell3 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell3.Border = 0; oPdfPCell3.Rowspan = 2;
                oPdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);
            }

            oPdfPCell3 = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle)); oPdfPCell3.Border = 0; 
            oPdfPCell3.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

            if (oEmployee.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmployee.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(52f, 60f);
                oPdfPCell3 = new PdfPCell(_oImag);
                oPdfPCell3.Rowspan = 3;
                oPdfPCell3.FixedHeight = 58;
                oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell3.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell3.PaddingLeft = 2;
                oPdfPCell3.Border = 0;

                oPdfPTable3.AddCell(oPdfPCell3);

            }
            else
            {
               
                oPdfPCell3 = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell3.Rowspan = 3;
                oPdfPCell3.FixedHeight = 50;
                oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell3.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell3.Border = 0;

                oPdfPTable3.AddCell(oPdfPCell3);

            }

            oPdfPTable3.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            oPdfPCell3 = new PdfPCell(new Phrase(_oCompany.FactoryAddress+"\nMobile No : "+_oCompany.Phone, _oFontStyle)); oPdfPCell3.Border = 0; 
            oPdfPCell3.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

            oPdfPTable3.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);

            oPdfPCell3 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell3.Border = 0; 
            oPdfPCell3.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

            oPdfPCell3 = new PdfPCell(new Phrase("IDENTITY CARD ", _oFontStyle)); oPdfPCell3.Border = 0; oPdfPCell3.PaddingTop = 4;
            oPdfPCell3.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

            oPdfPTable3.CompleteRow();

            oPdfPCell2 = new PdfPCell(oPdfPTable3); oPdfPCell2.Colspan = 4; oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);


            #region Employee Name 
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell2 = new PdfPCell(new Phrase("Name",_oFontStyle )); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.Name, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Blank Row
            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 1f; oPdfPCell2.Colspan = 4;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region ID & Date Of Birth
            oPdfPCell2 = new PdfPCell(new Phrase("ID", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.Code, _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.NORMAL);
            oPdfPCell2 = new PdfPCell(new Phrase("Date Of Birth", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DateOfBirth.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion
            
            #region Blank Row
            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 1f; oPdfPCell2.Colspan = 4;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Section & Issue Date
            oPdfPCell2 = new PdfPCell(new Phrase("Section", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DepartmentName, _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);


            oPdfPCell2 = new PdfPCell(new Phrase("Join Date", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3; 
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DateOfJoin.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Blank Row
            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 1f; oPdfPCell2.Colspan = 4;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Designation & B. Group
            oPdfPCell2 = new PdfPCell(new Phrase("Designation", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DesignationName, _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            
            oPdfPCell2 = new PdfPCell(new Phrase("Issue Date", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + _dIssueDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Blank Row
            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 1f; oPdfPCell2.Colspan = 4;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region B. Group
            oPdfPCell2 = new PdfPCell(new Phrase("B. Group", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.BloodGroupST, _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);


            oPdfPCell2 = new PdfPCell(new Phrase("Exp Date", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + _dExpireDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Blank Row
            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 1f; oPdfPCell2.Colspan = 4;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion

            #region Factory Mobile No
            //oPdfPCell2 = new PdfPCell(new Phrase("Factory Mobile No", _oFontStyle)); oPdfPCell2.Border = 0;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            //oPdfPCell2 = new PdfPCell(new Phrase(" : " + _oCompany.Phone, _oFontStyle)); oPdfPCell2.Border = 0;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);


            //oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            //oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            //oPdfPTable2.CompleteRow();
            #endregion

            #region Blank Row
            //oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 2f; oPdfPCell2.Colspan = 4;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            //oPdfPTable2.CompleteRow();
            #endregion

            #region Signature
            if (_oCompany.AuthSig != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.AuthSig, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 20f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 2;
                oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;
                oPdfPTable2.AddCell(oPdfPCell2);
            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2; oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            }

            if (oEmployee.EmployeeSiganture != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmployee.EmployeeSiganture, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(55f, 20f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 2;
                oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;
                oPdfPTable2.AddCell(oPdfPCell2);
            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2; oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            }
            oPdfPTable2.CompleteRow();
            #endregion

            #region Signature Caption
            oPdfPCell2 = new PdfPCell(new Phrase("Authorized Signature ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);


            oPdfPCell2 = new PdfPCell(new Phrase("Holder's Signature", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2; 
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            #endregion
            
            return oPdfPTable2;

        }

        #endregion
    }
}
