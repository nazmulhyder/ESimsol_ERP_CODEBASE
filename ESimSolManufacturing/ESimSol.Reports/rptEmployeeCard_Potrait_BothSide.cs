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
    public class rptEmployeeCard_Potrait_BothSide
    {
        #region Declaration

        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        Company _oCompany = new Company();
        DateTime _dIssueDate = DateTime.Today;
        DateTime _dExpireDate = DateTime.Today;
        #endregion

        public byte[] PrepareReport(Employee oEmployee, DateTime dIssueDate, DateTime dExpireDate)
        {
            _dIssueDate = dIssueDate;
            _dExpireDate = dExpireDate;
            _oEmployees = oEmployee.EmployeeHrms;
            _oEmployee = oEmployee;
            _oCompany = oEmployee.Company;

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(2f, 2f, 15f, 15f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            if (_oEmployee.Company.BaseAddress.ToUpper() == "B007")
            {
                _oPdfPTable.SetWidths(new float[] { 130f, 2f, 130f, 2f, 130f, 2f, 130f });
            }
            else
            {
                _oPdfPTable.SetWidths(new float[] { 140f, 2f, 140f, 2f, 140f, 2f, 140f });
            }
            
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
            _oPdfPCell.Colspan = 7;
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

            for (int i = 0; i < _oEmployees.Count; i = i + 2)
            {
                if (_oEmployee.Company.BaseAddress.ToUpper() == "B007")
                {
                    _oPdfPCell = new PdfPCell(GetEmployeeCard_FrontB007(_oEmployees[i]));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(GetEmployeeCard_Front(_oEmployees[i]));
                }
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (_oEmployee.Company.BaseAddress.ToUpper() == "B007")
                {
                    _oPdfPCell = new PdfPCell(GetEmployeeCard_BackB007(_oEmployees[i])); 
                }
                else
                {
                    _oPdfPCell = new PdfPCell(GetEmployeeCard_Back(_oEmployees[i])); 
                }                
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));  _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (i + 1 < _oEmployees.Count)
                {
                    if (_oEmployee.Company.BaseAddress.ToUpper() == "B007")
                    {
                        _oPdfPCell = new PdfPCell(GetEmployeeCard_FrontB007(_oEmployees[i + 1])); 
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(GetEmployeeCard_Front(_oEmployees[i + 1])); 
                    }                    
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (i + 1 < _oEmployees.Count)
                {
                    if (_oEmployee.Company.BaseAddress.ToUpper() == "B007")
                    {
                        _oPdfPCell = new PdfPCell(GetEmployeeCard_BackB007(_oEmployees[i + 1])); 
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(GetEmployeeCard_Back(_oEmployees[i + 1])); 
                    }                    
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Padding = 5; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }

        public PdfPTable GetEmployeeCard_Front(Employee oEmployee)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            PdfPCell oPdfPCell2;
            oPdfPTable2.SetWidths(new float[] { 40f, 40f, 75f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(141f, 28f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 3;
                oPdfPCell2.FixedHeight = 28;
                oPdfPCell2.PaddingLeft = -4;
                //oPdfPCell2.PaddingTop = -3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;

                oPdfPTable2.AddCell(oPdfPCell2);

            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            }

            oPdfPTable2.CompleteRow();

            if (IsExist(8)) //8 means BusinessUnit Address Front Side
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                oPdfPCell2 = new PdfPCell(new Phrase(oEmployee.BUAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                oPdfPCell2 = new PdfPCell(new Phrase("Tel:" + oEmployee.BUPhone, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();
            }
            else
            {
                if (IsExist(3))// 3 means company address  that is sent from the UI
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                    oPdfPCell2 = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    oPdfPCell2 = new PdfPCell(new Phrase("Tel:" + _oCompany.Phone, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();
                }
                else
                {
                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();
                }
            }

            


            if (oEmployee.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmployee.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 70f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 3;
                oPdfPCell2.FixedHeight = 70;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.PaddingLeft = 2;
                oPdfPCell2.Border = 0;

                oPdfPTable2.AddCell(oPdfPCell2);
            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell2.FixedHeight = 80;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;
                oPdfPCell2.Colspan = 3;
                oPdfPTable2.AddCell(oPdfPCell2);
            }

            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPCell2 = new PdfPCell(new Phrase("Card No", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            string sDateOfJoin = "";

            if (IsExist(1))// 1 means DateOfJoin that is sent from the UI
            {
                string sMonth = "";
                if (oEmployee.DateOfJoin.Month < 10)
                {
                    sMonth = "0" + oEmployee.DateOfJoin.Month;
                }
                else
                {
                    sMonth = oEmployee.DateOfJoin.Month.ToString();
                }
                sDateOfJoin = "/" + oEmployee.DateOfJoin.ToString("dd") + sMonth + oEmployee.DateOfJoin.ToString("yy");
            }

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.Code, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Name", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.Name, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Section", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DepartmentName, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Desig.", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DesignationName, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            if (IsExist(1))
            {
                oPdfPCell2 = new PdfPCell(new Phrase("Joining", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DateOfJoinInString, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }
            if (IsExist(6))
            {
                oPdfPCell2 = new PdfPCell(new Phrase("DOB", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DateOfBirthInString, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }
            if (IsExist(2))// 2 means Blood group  that is sent from the UI
            {
                oPdfPCell2 = new PdfPCell(new Phrase("B. Gr.", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.BloodGroupST, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }
            if (IsExist(4))// 4 means Issue Date that is sent from the UI
            {
                oPdfPCell2 = new PdfPCell(new Phrase("Iss. D.", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + DateTime.Now.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }


            if (_oCompany.AuthSig != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.AuthSig, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(30f, 22f);
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
                _oImag.ScaleAbsolute(30f, 22f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;

                oPdfPTable2.AddCell(oPdfPCell2);

            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            }
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Auth. Sign", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase("Holder's Sign", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }

        public PdfPTable GetEmployeeCard_FrontB007(Employee oEmployee)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            PdfPCell oPdfPCell2;
            oPdfPTable2.SetWidths(new float[] { 40f, 40f, 75f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(50f, 15f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 3;
                oPdfPCell2.FixedHeight = 20f;
                oPdfPCell2.PaddingLeft = -4;                
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;
                oPdfPTable2.AddCell(oPdfPCell2);
            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            }
            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD, new BaseColor(0, 134, 190)); //Baly Logo Color RGB (3, 129, 188)
            oPdfPCell2 = new PdfPCell(new Phrase(oEmployee.BusinessUnitName, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
                        
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCell2 = new PdfPCell(new Phrase("ID CARD", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
                        
            if (oEmployee.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmployee.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 70f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 3;
                oPdfPCell2.FixedHeight = 70;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.PaddingLeft = 2;
                oPdfPCell2.Border = 0;

                oPdfPTable2.AddCell(oPdfPCell2);
            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell2.FixedHeight = 80;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;
                oPdfPCell2.Colspan = 3;
                oPdfPTable2.AddCell(oPdfPCell2);
            }

            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPCell2 = new PdfPCell(new Phrase("Card No", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            string sDateOfJoin = "";

            if (IsExist(1))// 1 means DateOfJoin that is sent from the UI
            {
                string sMonth = "";
                if (oEmployee.DateOfJoin.Month < 10)
                {
                    sMonth = "0" + oEmployee.DateOfJoin.Month;
                }
                else
                {
                    sMonth = oEmployee.DateOfJoin.Month.ToString();
                }
                sDateOfJoin = "/" + oEmployee.DateOfJoin.ToString("dd") + sMonth + oEmployee.DateOfJoin.ToString("yy");
            }

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.Code, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Name", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.Name, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            //oPdfPCell2 = new PdfPCell(new Phrase("Section", _oFontStyle)); oPdfPCell2.Border = 0;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            //oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DepartmentName, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            //oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            //oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Desig.", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DesignationName, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            if (IsExist(1))
            {
                oPdfPCell2 = new PdfPCell(new Phrase("Joining", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DateOfJoinInString, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }
            if (IsExist(6))
            {
                oPdfPCell2 = new PdfPCell(new Phrase("DOB", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.DateOfBirthInString, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }
            if (IsExist(2))// 2 means Blood group  that is sent from the UI
            {
                oPdfPCell2 = new PdfPCell(new Phrase("B. Gr.", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + oEmployee.BloodGroupST, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }
            if (IsExist(4))// 4 means Issue Date that is sent from the UI
            {
                oPdfPCell2 = new PdfPCell(new Phrase("Iss. D.", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.PaddingLeft = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(" : " + _dIssueDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }


            if (_oCompany.AuthSig != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.AuthSig, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(30f, 22f);
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
                _oImag.ScaleAbsolute(30f, 22f);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;

                oPdfPTable2.AddCell(oPdfPCell2);

            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.FixedHeight = 22;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            }
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Auth. Sign", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 2;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase("Holder's Sign", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }

        public PdfPTable GetEmployeeCard_Back(Employee oEmployee)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            PdfPCell oPdfPCell2;
            oPdfPTable2.SetWidths(new float[] { 40f, 40f, 75f });

            if(IsExist(9)) //9 means Company Address Back Side
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                oPdfPCell2 = new PdfPCell(new Phrase("Factory Address:-", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCell2 = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                oPdfPCell2 = new PdfPCell(new Phrase("Tel:" + _oCompany.Phone, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();
            }
            else
            {
                if (IsExist(3))// 3 means company address  that is sent from the UI
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                    oPdfPCell2 = new PdfPCell(new Phrase("Factory Address:-", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                    oPdfPCell2 = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                    oPdfPCell2 = new PdfPCell(new Phrase("Tel:" + _oCompany.Phone, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();
                }
                else
                {
                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();
                }
            }

            

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell2 = new PdfPCell(new Phrase("Permanent Address : ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase(oEmployee.ParmanentAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Phone : "+oEmployee.ContactNo, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("National ID : " + oEmployee.NationalID, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3; oPdfPCell2.FixedHeight = 70;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCell2 = new PdfPCell(new Phrase("If ID Card Lost Please Inform " , _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("To Management Authority ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }

        public PdfPTable GetEmployeeCard_BackB007(Employee oEmployee)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            PdfPCell oPdfPCell2;
            oPdfPTable2.SetWidths(new float[] { 40f, 40f, 75f });

            if (IsExist(9)) //9 means Company Address Back Side
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                oPdfPCell2 = new PdfPCell(new Phrase("Factory Address:-", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                oPdfPCell2 = new PdfPCell(new Phrase(oEmployee.BUAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                oPdfPCell2 = new PdfPCell(new Phrase("Tel:" + oEmployee.BUPhone, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                oPdfPTable2.CompleteRow();
            }
            else
            {
                if (IsExist(3))// 3 means company address  that is sent from the UI
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                    oPdfPCell2 = new PdfPCell(new Phrase("Factory Address:-", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                    oPdfPCell2 = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                    oPdfPCell2 = new PdfPCell(new Phrase("Tel:" + _oCompany.Phone, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();
                }
                else
                {
                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();

                    oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
                    oPdfPTable2.CompleteRow();
                }
            }



            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell2 = new PdfPCell(new Phrase("Permanent Address : ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase(oEmployee.ParmanentAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Phone : " + oEmployee.ContactNo, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("National ID : " + oEmployee.NationalID, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3; oPdfPCell2.FixedHeight = 70;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCell2 = new PdfPCell(new Phrase("If ID Card Lost Please Inform ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("To Management Authority ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }

        public bool IsExist(int nId)
        {
            string[] oIDs;
            if (_oEmployee.ErrorMessage != "" && _oEmployee.ErrorMessage != null)
            {
                oIDs = _oEmployee.ErrorMessage.Split(',');
                foreach (string sId in oIDs)
                {
                    if (Convert.ToInt32(sId) == nId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
