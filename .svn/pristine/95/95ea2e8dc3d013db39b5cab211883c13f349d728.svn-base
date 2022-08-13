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

    public class rptESalaryStructure
    {
        #region Declaration
        int _nColumns = 0;
        int _nPageWidth = 0;
        int _npageHeight = 550;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable ;
        //= new PdfPTable(9)
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        EmployeeSalaryStructureDetail _oEmployeeSalaryStructureDetail = new EmployeeSalaryStructureDetail();
        List<EmployeeSalaryStructureDetail> _oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();

        Company _oCompany = new Company();
        List<SalaryHead> _oSalaryHeads = new List<SalaryHead>();
        List<EmployeeSalaryStructure> _oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();


        #endregion

        public byte[] PrepareReport(EmployeeSalaryStructureDetail oEmployeeSalaryStructureDetail)
        {
            _oEmployeeSalaryStructureDetails = oEmployeeSalaryStructureDetail.EmployeeSalaryStructureDetails;
            _oSalaryHeads = oEmployeeSalaryStructureDetail.SalaryHeads;
            _oEmployeeSalaryStructures = oEmployeeSalaryStructureDetail.EmployeeSalaryStructures;
            _oCompany = oEmployeeSalaryStructureDetail.Company;



            #region Page Setup
            _nColumns = _oSalaryHeads.Count + 3;

            float[] tablecolumns = new float[_nColumns];
           
            if (_nColumns <= 5)
            {
                _nPageWidth = 500;
                tablecolumns[0] = 20f;
                tablecolumns[1] = 130f;
            }
            else
            {
                _nPageWidth = 90 * (_nColumns);
                tablecolumns[0] = 15f;
                tablecolumns[1] = 120f;
            }

            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 400 / _nColumns ;
            }

           
            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            //_oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
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
            _oPdfPTable.HeaderRows = 6;
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
            _oPdfPCell.Colspan = _oSalaryHeads.Count+3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 3;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Employee Salary", _oFontStyle));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _oSalaryHeads.Count + 3;
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("SL No.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            foreach (SalaryHead oItem in _oSalaryHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Total Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
          
            
            foreach (EmployeeSalaryStructure oEItem in _oEmployeeSalaryStructures)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oEItem.EmployeeName + ", " + oEItem.DesignationName + ", " +oEItem.DepartmentName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nTotal = 0;
                foreach (SalaryHead oItem in _oSalaryHeads)
                {
                    
                        double nAm = 0;
                        nAm = GetAmount(oItem.SalaryHeadID, oEItem.ESSID);
                        nTotal = nTotal + nAm;
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAm), _oFontStyle));
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
            foreach(EmployeeSalaryStructureDetail oEDItem in _oEmployeeSalaryStructureDetails)
            {
                if (oEDItem.SalaryHeadID == nSHID && oEDItem.ESSID==nESSID)
                {
                    nAmount = oEDItem.Amount;
                }
                
            }
           
            return nAmount;
      
        }
        #endregion
    }




}
