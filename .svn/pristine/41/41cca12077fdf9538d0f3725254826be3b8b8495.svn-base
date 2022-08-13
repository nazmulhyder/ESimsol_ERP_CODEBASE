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
    public class rptEmployeeCard_Potrait_BothSideF2
    {
        #region Declaration
        float _nCardHeight = 0.0f;
        float _nCardWidth = 0.0f;
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
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
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842 X 595 || 1 Inch = 72.00 pxil
            _oDocument.SetMargins(32.5f, 32.5f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _nCardHeight = 245.0f;
            _nCardWidth = 160.0f;
            _oPdfPTable.SetWidths(new float[]  {
                                                        160.0f, 
                                                        30f, 
                                                        160.0f, 
                                                        30f, 
                                                        160.0f,
                                                });

            #endregion

            this.PrintBody();
            _oPdfPTable.HeaderRows = 0;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            for (int i = 0; i < _oEmployees.Count; i = i + 3)
            {
                _oPdfPCell = new PdfPCell(GetEmployeeCard_Front(_oEmployees[i]));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);


                if (i + 1 < _oEmployees.Count)
                {

                    _oPdfPCell = new PdfPCell(GetEmployeeCard_Front(_oEmployees[i + 1]));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                if (i + 2 < _oEmployees.Count)
                {
                    _oPdfPCell = new PdfPCell(GetEmployeeCard_Front(_oEmployees[i + 2]));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5;
                _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion

            for (int i = 0; i < _oEmployees.Count; i = i + 3)
            {
                if (i + 2 < _oEmployees.Count)
                {
                    _oPdfPCell = new PdfPCell(GetEmployeeCard_Back(_oEmployees[i + 2]));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                if (i + 1 < _oEmployees.Count)
                {

                    _oPdfPCell = new PdfPCell(GetEmployeeCard_Back(_oEmployees[i + 1]));

                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);

                }

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(GetEmployeeCard_Back(_oEmployees[i]));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.FixedHeight = _nCardHeight; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5;
                _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }

        public PdfPTable GetEmployeeCard_Front(Employee oEmployee)
        {
            PdfPCell oTempPdfCell = null;
            PdfPTable oTempPdfPTable = new PdfPTable(3);

            PdfPCell oPdfPCellFront = null;
            PdfPTable oFrontTable = new PdfPTable(3);
            oFrontTable.SetWidths(new float[] { 45f, 10f, 110f });
            oFrontTable.WidthPercentage = 100;
                        
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);

            #region Company Logo & Address
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(141f, 15f);
                oPdfPCellFront = new PdfPCell(_oImag);
                oPdfPCellFront.Colspan = 3;
                oPdfPCellFront.FixedHeight = 15f;
                oPdfPCellFront.PaddingLeft = -4;                
                oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellFront.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellFront.Border = 0;
                oFrontTable.AddCell(oPdfPCellFront);
            }
            else
            {
                oPdfPCellFront = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3;
                oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            }
            oFrontTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            oPdfPCellFront = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle)); oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();

            oPdfPCellFront = new PdfPCell(new Phrase("Tel:" + _oCompany.Phone, _oFontStyle)); oPdfPCellFront.BorderWidthLeft = oPdfPCellFront.BorderWidthRight = 0; oPdfPCellFront.BorderWidthTop = 0; oPdfPCellFront.BorderWidthBottom = 1; oPdfPCellFront.Colspan = 3;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();

            oPdfPCellFront = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Blank Row
            //oPdfPCellFront = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3; oPdfPCellFront.FixedHeight = 2f;
            //oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            //oFrontTable.CompleteRow();
            #endregion
            
            oTempPdfPTable = new PdfPTable(3);
            oTempPdfPTable.SetWidths(new float[] { 30f, 100f, 30f });
            oTempPdfPTable.WidthPercentage = 100;

            #region ID CARD Hading
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oTempPdfCell = new PdfPCell(new Phrase("", _oFontStyle)); oTempPdfCell.Border = 0;
            oTempPdfCell.HorizontalAlignment = Element.ALIGN_CENTER; oTempPdfCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(oTempPdfCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oTempPdfCell = new PdfPCell(new Phrase("ID CARD", _oFontStyle));
            oTempPdfCell.FixedHeight = 15f;
            oTempPdfCell.Padding = 2f; oTempPdfCell.Border = 0;
            oTempPdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;            
            oTempPdfCell.CellEvent = new RoundedBorder();
            oTempPdfCell.HorizontalAlignment = Element.ALIGN_CENTER; oTempPdfCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(oTempPdfCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oTempPdfCell = new PdfPCell(new Phrase("", _oFontStyle)); oTempPdfCell.Border = 0;
            oTempPdfCell.HorizontalAlignment = Element.ALIGN_CENTER; oTempPdfCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(oTempPdfCell);
            oTempPdfPTable.CompleteRow();

            #region Insert Into Front Table 
            oPdfPCellFront = new PdfPCell(oTempPdfPTable);
            oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion
            #endregion

            #region Blank Row
            oPdfPCellFront = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3; oPdfPCellFront.FixedHeight = 2f;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Employee Image
            oTempPdfPTable = new PdfPTable(3);
            oTempPdfPTable.SetWidths(new float[] { 40f, 80f, 40f });
            oTempPdfPTable.WidthPercentage = 100;

            oTempPdfCell = new PdfPCell(new Phrase("", _oFontStyle));
            oTempPdfCell.FixedHeight = 80; oTempPdfCell.Border = 0; oTempPdfPTable.AddCell(oTempPdfCell);

            if (oEmployee.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmployee.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 70f);
                oTempPdfCell = new PdfPCell(_oImag);
                oTempPdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTempPdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                oTempPdfCell.FixedHeight = 80;
                oTempPdfCell.Padding = 2.5f;
                oTempPdfPTable.AddCell(oTempPdfCell);
            }
            else
            {
                oTempPdfCell = new PdfPCell(new Phrase("", _oFontStyle));
                oTempPdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oTempPdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                oTempPdfCell.FixedHeight = 80;
                oTempPdfCell.Padding = 2.5f;
                oTempPdfPTable.AddCell(oTempPdfCell);
            }

            oTempPdfCell = new PdfPCell(new Phrase("", _oFontStyle));
            oTempPdfCell.FixedHeight = 80; oTempPdfCell.Border = 0; oTempPdfPTable.AddCell(oTempPdfCell);
            oTempPdfPTable.CompleteRow();


            #region Insert Into Front Table
            oPdfPCellFront = new PdfPCell(oTempPdfPTable);
            oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion
            #endregion

            #region Blank Row
            oPdfPCellFront = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3; oPdfPCellFront.FixedHeight = 2f;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion
                                    
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region ID No
            oPdfPCellFront = new PdfPCell(new Phrase(" ID No", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(oEmployee.Code, _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Name
            oPdfPCellFront = new PdfPCell(new Phrase(" Name", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(oEmployee.Name, _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Desig
            oPdfPCellFront = new PdfPCell(new Phrase(" Desig", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(oEmployee.DesignationName, _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Section
            oPdfPCellFront = new PdfPCell(new Phrase(" Section", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(oEmployee.DepartmentName, _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Join Date
            oPdfPCellFront = new PdfPCell(new Phrase(" Join Date", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);

            oPdfPCellFront = new PdfPCell(new Phrase(oEmployee.DateOfJoinInString, _oFontStyle)); oPdfPCellFront.Border = 0;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion

            #region Blank Row
            oPdfPCellFront = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3; oPdfPCellFront.FixedHeight = 20f;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion
           
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region Signature
            oTempPdfPTable = new PdfPTable(3);
            oTempPdfPTable.SetWidths(new float[] { 60f, 40f, 60f });
            oTempPdfPTable.WidthPercentage = 100;

            oTempPdfCell = new PdfPCell(new Phrase(" Holder's Sign", _oFontStyle)); oTempPdfCell.PaddingTop = 2f;
            oTempPdfCell.BorderWidthLeft = oTempPdfCell.BorderWidthRight = oTempPdfCell.BorderWidthBottom = 0; oTempPdfCell.BorderWidthTop = 1;
            oTempPdfCell.HorizontalAlignment = Element.ALIGN_LEFT; oTempPdfCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(oTempPdfCell);

            oTempPdfCell = new PdfPCell(new Phrase("", _oFontStyle)); oTempPdfCell.Border = 0; oTempPdfCell.PaddingTop = 2f;    
            oTempPdfCell.HorizontalAlignment = Element.ALIGN_LEFT; oTempPdfCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(oTempPdfCell);

            oTempPdfCell = new PdfPCell(new Phrase("Authority Sign ", _oFontStyle)); oTempPdfCell.PaddingTop = 2f;
            oTempPdfCell.BorderWidthLeft = oTempPdfCell.BorderWidthRight = oTempPdfCell.BorderWidthBottom = 0; oTempPdfCell.BorderWidthTop = 1;
            oTempPdfCell.HorizontalAlignment = Element.ALIGN_RIGHT; oTempPdfCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(oTempPdfCell);
            oTempPdfPTable.CompleteRow();


            #region Insert Into Front Table
            oPdfPCellFront = new PdfPCell(oTempPdfPTable);
            oPdfPCellFront.Border = 0; oPdfPCellFront.Colspan = 3;
            oPdfPCellFront.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellFront.BackgroundColor = BaseColor.WHITE; oFrontTable.AddCell(oPdfPCellFront);
            oFrontTable.CompleteRow();
            #endregion
            #endregion

            return oFrontTable;
        }

        public PdfPTable GetEmployeeCard_Back(Employee oEmployee)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(3);
            PdfPCell oPdfPCell2;
            oPdfPTable2.SetWidths(new float[] { 40f, 40f, 75f });
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell2 = new PdfPCell(new Phrase("If this card found, please return", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3; oPdfPCell2.PaddingTop = 10;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("To following Authority ", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            if (_oCompany.OrganizationTitleLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.OrganizationTitleLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(75, 60);
                oPdfPCell2 = new PdfPCell(_oImag);
                oPdfPCell2.Colspan = 3;
                oPdfPCell2.FixedHeight = 80;
                oPdfPCell2.PaddingLeft = -4;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell2.Border = 0;
                oPdfPTable2.AddCell(oPdfPCell2);
            }
            else
            {
                oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3; oPdfPCell2.FixedHeight = 80;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2); oPdfPCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            }
            oPdfPTable2.CompleteRow();


            oPdfPCell2 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            oPdfPCell2 = new PdfPCell(new Phrase(oEmployee.ParmanentAddress, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2); oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Tel : " + oEmployee.ContactNo, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3; oPdfPCell2.PaddingLeft = 30;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Fax : " + oEmployee.BUFaxNo, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3; oPdfPCell2.PaddingLeft = 30;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            oPdfPCell2 = new PdfPCell(new Phrase("Blood Group : " + oEmployee.BloodGroup, _oFontStyle)); oPdfPCell2.Border = 0; oPdfPCell2.Colspan = 3;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();
            return oPdfPTable2;
        }        
        #endregion
    }
}