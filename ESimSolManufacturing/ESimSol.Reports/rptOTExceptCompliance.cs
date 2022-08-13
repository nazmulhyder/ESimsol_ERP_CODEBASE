
using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ESimSol.Reports
{
    public class rptOTExceptCompliance
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        int _nColumn = 11;
        PdfPTable _oPdfPTable = new PdfPTable(11);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<OTExceptCompliance> _oOTExceptCompliances = new List<OTExceptCompliance>();
        List<OTExceptCompliance> _oOTExceptCompliancesT = new List<OTExceptCompliance>();
        Company _oCompany = new Company();
        List<PayrollProcessManagement> _oPayrollProcessManagements = new List<PayrollProcessManagement>();
        string _sStartDate = "";
        string _sEndDate = "";
        bool _bExceptComp = false;
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        bool _isComp;
        bool _bGroupDept;
        #endregion

        public byte[] PrepareReport(List<OTExceptCompliance> oOTExceptCompliances, Company oCompany, List<PayrollProcessManagement> oPayrollProcessManagements, string sDate, List<SalarySheetSignature> oSalarySheetSignatures, bool bExceptComp, List<BusinessUnit> oBusinessUnits, bool IsComp, bool bGroupByDept)
        {
            _isComp = IsComp;
            _bGroupDept = bGroupByDept;
            //_oOTExceptCompliancesT = oOTExceptCompliances;
            if (_isComp)
            {
                _oOTExceptCompliances = oOTExceptCompliances.Where(x=>x.CompOTInMinute > 0).OrderBy(x=>x.DepartmentName).ToList();
            }
            else
            {
                _oOTExceptCompliances = oOTExceptCompliances.OrderBy(x => x.DepartmentName).ToList();
            }
            _oCompany = oCompany;
            _oPayrollProcessManagements = oPayrollProcessManagements;
            if (_oPayrollProcessManagements.Count > 0)
            {
                DateTime sStartDate = Convert.ToDateTime(sDate.Split(',')[0]);
                DateTime sEndDate = Convert.ToDateTime(sDate.Split(',')[1]);
                _sStartDate = sStartDate.ToString("dd MMM yyyy");
                _sEndDate = sEndDate.ToString("dd MMM yyyy");
            }
            _bExceptComp = bExceptComp;
            _oBusinessUnits = oBusinessUnits;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(30f, 30f, 10f, 70f);
            _oPdfPTable.WidthPercentage = 97;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            oPDFWriter.PageEvent = PageEventHandler;

            //PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 28f, 40f, 100f, 70f, 50f, 50f, 45f, 35f, 40f, 45f, 60f });
            #endregion

            //this.PrintHeader();
            this.Body();
            //_oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 17f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnits.Count == 1 ? _oBusinessUnits[0].Name : _oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Title
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("OT Sheet", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 8f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Month-" + (_oPayrollProcessManagements.Count > 0 ? _oPayrollProcessManagements[0].MonthIDInString : ""), _oFontStyle));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region Report Body

        private void Header(string sLoc, string sDept)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Unit : " + sLoc + ", Department : " + sDept, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            this.TableHead();
        }
        public void Body()
        {
            List<OTExceptCompliance> oOTExceptCompliances = new List<OTExceptCompliance>();
            _oOTExceptCompliances.ForEach(x => oOTExceptCompliances.Add(x));

            if (_oOTExceptCompliances.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(""));
                _oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to Print!"));
                _oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                while (oOTExceptCompliances.Count > 0)
                {
                    var oResults = new List<OTExceptCompliance>();
                    if (_bGroupDept)
                    {
                        oResults = oOTExceptCompliances.Where(x => x.LocationID == oOTExceptCompliances[0].LocationID && x.DepartmentName == oOTExceptCompliances[0].DepartmentName).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();
                    }
                    else
                    {
                        oResults = oOTExceptCompliances.Where(x => x.LocationID == oOTExceptCompliances[0].LocationID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.Code).ToList();
                    }

                    //this.Header(oResults[0].BusinessUnitName, oResults[0].DepartmentName);
                    this.PrintHeader();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    if (_bGroupDept)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Location Name: " + oResults.FirstOrDefault().LocationName + ", Department Name : " + oResults.FirstOrDefault().DepartmentName, _oFontStyle));
                        _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Location Name: " + oResults.FirstOrDefault().LocationName, _oFontStyle));
                        _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    this.TableHead();


                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    this.PrintBody(oResults);

                    //this.Summary(oResults);

                    if (_bGroupDept)
                        oOTExceptCompliances.RemoveAll(x => x.LocationID == oResults[0].LocationID && x.DepartmentName == oResults[0].DepartmentName);
                    else
                        oOTExceptCompliances.RemoveAll(x => x.LocationID == oResults[0].LocationID);
                }
            }
            //if (_oOTExceptCompliances.Count > 0)
            //{
            //    this.GrandSummary();
            //}
        }
        int nCount = 0;
        int nCountForEnd = 0;
        int nRowCount=0;
        int nTotalCount = 0;
        float nRowHeight = 23f;


        int nTotalRowCount = 0;
        int UnitWiseRowCount = 0;
        private void PrintBody(List<OTExceptCompliance> oOTExceptCompliance)
        {
            nCountForEnd = 0;
            foreach (OTExceptCompliance oItem in oOTExceptCompliance)
            {
                nCount++;
                nRowCount++;
                nTotalCount++;
                nTotalRowCount++;
                UnitWiseRowCount++;
                nCountForEnd++;

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                _oPdfPCell = new PdfPCell(new Phrase((nCount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Code, _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DesignationName, _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DateOfJoinInStr, _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((_isComp ? GetAmountInStr(oItem.CompGrossAmount, true, false) : GetAmountInStr(oItem.GrossAmount, true, false)), _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GetAmountInStr(oItem.BasicAmount, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((_isComp) ? Global.MillionFormat(oItem.CompOTRatePerHour) : Global.MillionFormat(oItem.OTRatePerHour), _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((_isComp?oItem.CompOTHour:_bExceptComp?oItem.AdditionalOTHour: oItem.OTHour)), _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GetAmountInStr((_isComp ? oItem.CompOTAmount : (_bExceptComp)?oItem.OTAmountExceptComp:oItem.OTAmount), true, false), _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                bool bEndDept = false;
                if (nCountForEnd == oOTExceptCompliance.Count())
                    bEndDept = true;
                var nModBy = 27;
                if (nCountForEnd % nModBy == 0)
                {

                    if (UnitWiseRowCount != oOTExceptCompliance.Count)
                    {
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        nCountForEnd = 0;
                        this.PrintHeader();
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                        if (_bGroupDept)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("Location Name: " + oItem.LocationName + ", Department Name : " + oItem.DepartmentName, _oFontStyle));
                            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("Location Name: " + oItem.LocationName, _oFontStyle));
                            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        _oPdfPTable.CompleteRow();
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        this.TableHead();
                    }
                }

                //var nMod = nRowCount % 27;
                //if (nMod == 0 || oOTExceptCompliances.Count == nCount)
                //{
                //    if (oOTExceptCompliances.LastOrDefault().EmployeeID == oItem.EmployeeID)
                //    {
                //        this.Summary(oOTExceptCompliances);
                //        nRowCount++;
                //        nLocation++;
                //        nCount = 0;
                //    }
                //    nRowCount = 0;
                //    _oDocument.Add(_oPdfPTable);
                //    _oDocument.NewPage();
                //    _oPdfPTable.DeleteBodyRows();
                //    if (_oOTExceptCompliances.Count != nTotalCount)
                //    {
                //        this.PrintHeader(); 
                //        //this.Header(_oOTExceptCompliances.Where(x => x.LocationID == arr[nLocation]).ToList()[0].LocationName);
                //    }

                //}
            }

            this.Summary(oOTExceptCompliance);
            if (_oOTExceptCompliances.Count == nCount)
            {
                this.GrandSummary();
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
        }

        private void TableHead()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Joining ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Basic ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT Rate", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT Hour", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        private void Summary(List<OTExceptCompliance> oOTExceptCompliances)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_isComp ? this.GetAmountInStr(oOTExceptCompliances.Sum(x => x.CompGrossAmount), true, true) : this.GetAmountInStr(oOTExceptCompliances.Sum(x => x.GrossAmount), true, true)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oOTExceptCompliances.Sum(x => x.BasicAmount)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            string sTotalOTHour="";
            if (_isComp)
            {
                sTotalOTHour = this.GetAmountInStr(oOTExceptCompliances.Sum(x => x.CompOTHour),true,true);
            }
            else
            {
                if (_bExceptComp)
                {
                    sTotalOTHour = this.GetAmountInStr(oOTExceptCompliances.Sum(x => x.AdditionalOTHour),true,true);
                }
                else
                { sTotalOTHour = this.GetAmountInStr(oOTExceptCompliances.Sum(x => x.OTHour),true,true); }
            }

            _oPdfPCell = new PdfPCell(new Phrase(sTotalOTHour, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr((_isComp ? oOTExceptCompliances.Sum(x => x.CompOTAmount) : (_bExceptComp)?oOTExceptCompliances.Sum(x => x.OTAmountExceptComp):oOTExceptCompliances.Sum(x => x.OTAmount)),true,true), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;  _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void GrandSummary()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_isComp ? this.GetAmountInStr(_oOTExceptCompliances.Sum(x => x.CompGrossAmount), true, true) : this.GetAmountInStr(_oOTExceptCompliances.Sum(x => x.GrossAmount), true, true)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oOTExceptCompliances.Sum(x => x.BasicAmount)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            string sTotalOTHour = "";
            if (_isComp)
            {
                sTotalOTHour = this.GetAmountInStr(_oOTExceptCompliances.Sum(x => x.CompOTHour), true, true);
            }
            else
            {
                if (_bExceptComp)
                {
                    sTotalOTHour = this.GetAmountInStr(_oOTExceptCompliances.Sum(x => x.AdditionalOTHour), true, true);
                }
                else
                { sTotalOTHour = this.GetAmountInStr(_oOTExceptCompliances.Sum(x => x.OTHour), true, true); }
            }

            _oPdfPCell = new PdfPCell(new Phrase(sTotalOTHour, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr((_isComp ? _oOTExceptCompliances.Sum(x => x.CompOTAmount) : (_bExceptComp) ? _oOTExceptCompliances.Sum(x => x.OTAmountExceptComp) : _oOTExceptCompliances.Sum(x => x.OTAmount)), true, true), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 16f; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        #endregion


        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = Math.Round(amount, 0);
            return amount.ToString();
            //return (bWithPrecision) ? Global.MillionFormat(amount).Split('.')[0] : Global.MillionFormat(amount);
        }

    }
}
