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
    public class rptMonthlyProductionCard
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        iTextSharp.text.Image _oImag;
        EmployeeProduction _oEmployeeProduction = new EmployeeProduction();
        List<EmployeeProduction> _oEmployeeProductions = new List<EmployeeProduction>();
        List<EmployeeProductionReceiveDetail> _oEmployeeProductionReceiveDetails = new List<EmployeeProductionReceiveDetail>();
        Company _oCompany = new Company();


        //string sHeader = "";
        string sDateFrom = "";
        string sDateTo = "";

        #endregion

        public byte[] PrepareReport(EmployeeProduction oEmployeeProduction)
        {
            _oEmployeeProductions = oEmployeeProduction.EmployeeProductions;
            _oEmployeeProduction = oEmployeeProduction;
            _oEmployeeProductionReceiveDetails = oEmployeeProduction.EmployeeProductionReceiveDetails;
            _oCompany = oEmployeeProduction.Company;


            //if (oEmployeeProduction.ProductionProcess == EnumProductionProcess.Knitting)
            //{
            //    sHeader = "Knitting Section";
            //}

            sDateFrom = oEmployeeProduction.ErrorMessage.Split(',')[0];
            sDateTo = oEmployeeProduction.ErrorMessage.Split(',')[1];

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
            _oPdfPTable.SetWidths(new float[] { 30f, 90f, 100f, 55f, 45f, 60f, 50f, 45f, 60f });
            #endregion

            _oDocument.AddHeader("Header", "Header Text");
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

            //if (_oCompany.CompanyLogo != null)
            //{
            //    _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    _oImag.ScaleAbsolute(28f, 22f);
            //    _oPdfPCell = new PdfPCell(_oImag);
            //    _oPdfPCell.Rowspan = 2;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);

            //}
            //else
            //{

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPCell.Border = 0;
            //    _oPdfPCell.Rowspan = 2;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.ExtraParagraphSpace = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);

            //}

            //_oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.PaddingTop = 6;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.Rowspan = 2;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);
            //_oPdfPCell = new PdfPCell(new Phrase("MONTHLY PRODUCTION (" + sHeader + ")", _oFontStyle));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.Rowspan = 2;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Machine No : " + _oEmployeeProductions[0].MachineNo, _oFontStyle));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Name Of Operator : " + _oEmployeeProductions[0].EmployeeName, _oFontStyle));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;

            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Date From " + sDateFrom + " To " + sDateTo, _oFontStyle));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Card No : " + _oEmployeeProductions[0].Code, _oFontStyle));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

            PdfPTable oPdfPTableHeader = new PdfPTable(3);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 40f, 360f });
            PdfPCell oPdfPCellHearder;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCellHearder = new PdfPCell(new Phrase(/*"Name :" + _oEmployeeProductions[0].EmployeeName + "\nCode :" + _oEmployeeProductions[0].Code*/"", _oFontStyle));
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("MONTHLY PRODUCTION", _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Date From " + sDateFrom + " To " + sDateTo, _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 3;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body

        private void PrintBody()
        {
            //List<EmployeeProduction> oEmployeeProductions=new List<EmployeeProduction>();
            if (_oEmployeeProductions.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to print!!", _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }

            while (_oEmployeeProductions.Count > 0)
            {
                List<EmployeeProduction> oTempEmployeeProductions = new List<EmployeeProduction>();
                oTempEmployeeProductions = _oEmployeeProductions.Where(x => x.EmployeeID == _oEmployeeProductions[0].EmployeeID).ToList();

                List<int> EPSIDs = new List<int>();
                EPSIDs = oTempEmployeeProductions.Select(x => x.EPSID).ToList();
                List<EmployeeProductionReceiveDetail> oEmployeeProductionReceiveDetails = new List<EmployeeProductionReceiveDetail>();
                List<EmployeeProductionReceiveDetail> oEPRDs = new List<EmployeeProductionReceiveDetail>();
                oEmployeeProductionReceiveDetails = (from oEPRDetail in _oEmployeeProductionReceiveDetails
                                                     where EPSIDs.Contains(oEPRDetail.EPSID)
                                                     orderby oEPRDetail.StyleNo
                                                     select oEPRDetail).ToList();
                oEPRDs.AddRange(oEmployeeProductionReceiveDetails);
                BodyPartCount(oEPRDs);
                PrintEmployeeProductionReceiveDetail(oEmployeeProductionReceiveDetails, _oEmployeeProductions[0].EmployeeNameCode);
                _oEmployeeProductions.RemoveAll(x => x.EmployeeID == oTempEmployeeProductions[0].EmployeeID);
            }

        }
        int nCount = 0;

        private void PrintEmployeeProductionReceiveDetail(List<EmployeeProductionReceiveDetail> oEPReceiveDetails, string sName)
        {

            nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Employee Name : " + sName, _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.BorderWidthBottom = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SL NO", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.BorderWidthBottom = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("STYLE", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.BorderWidthBottom = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPCell.BorderWidthBottom = 0; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BODY PART", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("SIZE", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NOTE", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RCV. QTY.", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RATE", _oFontStyle)); _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AMOUNT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            double nAmount = 0;
            double nTotalAmount = 0;
            double nTotalRCVPCS = 0;

            //==============
            var oTemp = (from NewEPRD in oEPReceiveDetails
                         group NewEPRD by new { NewEPRD.StyleNo, NewEPRD.GPName, NewEPRD.ProductionNote, NewEPRD.NewRate } into grp
                         select new
                         {
                             StyleNo = grp.Key.StyleNo,
                             ColorName = string.Join(",", grp.Select(x => x.ColorName).Distinct()),
                             BodyPart = grp.Key.GPName,
                             Size = string.Join(",", grp.Select(x => x.SizeCategoryName).Distinct()),
                             //Size = grp.Key.SizeCategoryName,
                             RcvQty = grp.Sum(x => x.RcvQty),
                             Rate = grp.Key.NewRate,
                             //ProductionNote = grp.Select(x => x.ProductionNote),
                             ProductionNote = grp.Key.ProductionNote

                         }).ToList();


            List<EmployeeProductionReceiveDetail> oEmployeeProductionReceiveDetails = new List<EmployeeProductionReceiveDetail>();
            EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetail = new EmployeeProductionReceiveDetail();
            if (oTemp.Count > 0)
            {
                foreach (var oItem in oTemp)
                {
                    oEmployeeProductionReceiveDetail = new EmployeeProductionReceiveDetail();
                    oEmployeeProductionReceiveDetail.StyleNo = oItem.StyleNo;
                    oEmployeeProductionReceiveDetail.ColorName = oItem.ColorName;
                    oEmployeeProductionReceiveDetail.ErrorMessage = oItem.BodyPart;
                    oEmployeeProductionReceiveDetail.SizeCategoryName = oItem.Size;
                    oEmployeeProductionReceiveDetail.RcvQty = oItem.RcvQty;
                    oEmployeeProductionReceiveDetail.Rate = oItem.Rate;
                    oEmployeeProductionReceiveDetail.ProductionNote = oItem.ProductionNote;
                    oEmployeeProductionReceiveDetails.Add(oEmployeeProductionReceiveDetail);

                }

            }

            //==============

            foreach (EmployeeProductionReceiveDetail oItem in oEmployeeProductionReceiveDetails)
            {
                nCount++;
                nAmount = oItem.RcvQty * oItem.Rate;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ErrorMessage, _oFontStyle));//BodyPart
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SizeCategoryName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductionNote, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.RcvQty.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                nTotalAmount += nAmount;
                nTotalRCVPCS += oItem.RcvQty;
                _oPdfPTable.CompleteRow();
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalRCVPCS), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(sEmployeeWiseString, _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        #endregion

        //double nFullReceive = 0;
        //double nFrontReceive = 0;
        //double nBackReceive = 0;
        //double nSleeve_2Receive = 0;
        //double nFrontPart_2Receive = 0;
        //double nBackPart_2Receive = 0;
        //double nNeckReceive = 0;
        //double nNeckBigReceive = 0;
        //double nNeckSmallReceive = 0;
        //double nPlacket_2Receive = 0;
        //double nPiping_2Receive = 0;
        //double nHood_2Receive = 0;
        //double nHoodReceive = 0;
        //double nHoodRibReceive = 0;
        //double nPocketReceive = 0;
        //double nPocket_2Receive = 0;
        //double nPocketRib_2Receive = 0;
        //double nPocketBeg_2Receive = 0;
        //double nShoulderPatch_2Receive = 0;
        //double nElbowPatch_2Receive = 0;
        //double nMoonReceive = 0;
        //double nBottomRibReceive = 0;
        //double nArmholeReceive = 0;
        //double nFashionReceive = 0;
        //string sStyleNoStart = "";
        //string sStyleNoEnd = "";
        //bool bFlag = true;
        string sEmployeeWiseString = "";

        private void BodyPartCount(List<EmployeeProductionReceiveDetail> oLists)
        {
            sEmployeeWiseString = "";

            while (oLists.Count > 0)
            {
                List<EmployeeProductionReceiveDetail> oTempEPRDs = new List<EmployeeProductionReceiveDetail>();
                oTempEPRDs = oLists.Where(x => x.StyleNo == oLists[0].StyleNo).ToList();

                string sStyleWiseString = "";
                string sStyleNo = oTempEPRDs[0].StyleNo;
                while (oTempEPRDs.Count > 0)
                {
                    double nRcvQty = 0;
                    List<EmployeeProductionReceiveDetail> oTempBPartEPRDs = new List<EmployeeProductionReceiveDetail>();
                    oTempBPartEPRDs = oLists.Where(x => x.GarmentPart == oTempEPRDs[0].GarmentPart).ToList();

                    foreach (EmployeeProductionReceiveDetail oItem in oTempBPartEPRDs)
                    {
                        nRcvQty += oItem.RcvQty;
                    }
                    sStyleWiseString += oLists[0].StyleNo + "/" + oTempEPRDs[0].GPName + "-" + nRcvQty.ToString();
                    oTempEPRDs.RemoveAll(x => x.GarmentPart == oTempBPartEPRDs[0].GarmentPart);
                }

                sEmployeeWiseString += "(" + sStyleWiseString + ")";
                oLists.RemoveAll(x => x.StyleNo == sStyleNo);

            }

        }

        //string sStyleWiseString = "";
        //private string CountString()
        //{
        //    string sFullString = "";
        //    string sFrontString = "";
        //    string sBackString = "";
        //    string sSleeve_2String = "";
        //    string sFrontPart_2String = "";
        //    string sBackPart_2String = "";
        //    string sNeckString = "";
        //    string sNeckBigString = "";
        //    string sNeckSmallString = "";
        //    string sPlacket_2String = "";
        //    string sPiping_2String = "";
        //    string sHood_2String = "";
        //    string sHoodString = "";
        //    string sHoodRibString = "";
        //    string sPocketString = "";
        //    string sPocket_2String = "";
        //    string sPocketRib_2String = "";
        //    string sPocketBeg_2String = "";
        //    string sShoulderPatch_2String = "";
        //    string sElbowPatch_2String = "";
        //    string sMoonString = "";
        //    string sBottomRibString = "";
        //    string sArmholeString = "";
        //    string sFashionString = "";


        //    if (nFullReceive != 0)
        //    {
        //        sFullString = "Full-" + nFullReceive;

        //    }
        //    if (nFrontReceive != 0)
        //    {
        //        sFrontString = "Front-" + nFrontReceive;

        //    }

        //    if (nBackReceive != 0)
        //    {
        //        sBackString = "Back-" + nBackReceive;

        //    }
        //    if (nSleeve_2Receive != 0)
        //    {
        //        sSleeve_2String = "Sleeve_2-" + nSleeve_2Receive;

        //    }
        //    if (nFrontPart_2Receive != 0)
        //    {
        //        sFrontPart_2String = "FrontPart_2-" + nFrontPart_2Receive;

        //    }
        //    if (nBackPart_2Receive != 0)
        //    {
        //        sBackPart_2String = "BackPart_2-" + nBackPart_2Receive;

        //    }
        //    else if (nNeckReceive != 0)
        //    {
        //        sNeckString = "Neck-" + nNeckReceive;

        //    }
        //    if (nNeckBigReceive != 0)
        //    {
        //        sNeckBigString = "NeckBig-" + nNeckBigReceive;

        //    }
        //    if (nNeckSmallReceive != 0)
        //    {
        //        sNeckSmallString = "NeckSmall-" + nNeckSmallReceive;

        //    }
        //    if (nPlacket_2Receive != 0)
        //    {
        //        sPlacket_2String = "Placket_2-" + nPlacket_2Receive;

        //    }
        //    if (nPiping_2Receive != 0)
        //    {
        //        sPiping_2String = "Piping_2-" + nPiping_2Receive;

        //    }

        //    if (nHood_2Receive != 0)
        //    {
        //        sHood_2String = "Hood_2-" + nHood_2Receive;

        //    }
        //    if (nHoodReceive != 0)
        //    {
        //        sHood_2String = "Hood-" + nHoodReceive;

        //    }
        //    if (nHoodRibReceive != 0)
        //    {
        //        sHoodRibString = "HoodRib-" + nHoodRibReceive;

        //    }
        //    if (nPocketReceive != 0)
        //    {
        //        sPocketString = "Pocket-" + nPocketReceive;

        //    }
        //    if (nPocket_2Receive != 0)
        //    {
        //        sPocket_2String = "Pocket_2-" + nPocket_2Receive;

        //    }
        //    if (nPocketRib_2Receive != 0)
        //    {
        //        sPocketRib_2String = "PocketRib_2-" + nPocketRib_2Receive;

        //    }
        //    if (nPocketBeg_2Receive != 0)
        //    {
        //        sPocketBeg_2String = "PocketBeg_2-" + nPocketBeg_2Receive;

        //    }
        //    if (nShoulderPatch_2Receive != 0)
        //    {
        //        sShoulderPatch_2String = "ShoulderPatch_2-" + nShoulderPatch_2Receive;

        //    }
        //    if (nElbowPatch_2Receive != 0)
        //    {
        //        sElbowPatch_2String = "ElbowPatch_2-" + nElbowPatch_2Receive;

        //    }
        //    if (nMoonReceive != 0)
        //    {
        //        sMoonString = "Moon-" + nMoonReceive;

        //    }
        //    if (nBottomRibReceive != 0)
        //    {
        //        sBottomRibString = "BottomRib-" + nBottomRibReceive;

        //    }
        //    if (nArmholeReceive != 0)
        //    {
        //        sArmholeString = "Armhole-" + nArmholeReceive;

        //    }
        //    if (nFashionReceive != 0)
        //    {
        //        sFashionString = "Fashion-" + nFashionReceive;

        //    }

        //    sStyleWiseString = sStyleNoEnd + "/" + sFullString + sFrontString + sBackString + sSleeve_2String + sFrontPart_2String + sBackPart_2String + sNeckString + sNeckBigString + sNeckSmallString + sPlacket_2String + sPiping_2String + sHood_2String + sHoodString + sHoodRibString + sPocketString + sPocket_2String + sPocketRib_2String + sPocketBeg_2String + sShoulderPatch_2String + sElbowPatch_2String + sMoonString + sBottomRibString + sArmholeString + sFashionString;
        //    return sStyleWiseString;
        //}
        //private void InitializeIntigerType()
        //{
        //    nFullReceive = 0;
        //    nFrontReceive = 0;
        //    nBackReceive = 0;
        //    nSleeve_2Receive = 0;
        //    nFrontPart_2Receive = 0;
        //    nBackPart_2Receive = 0;
        //    nNeckReceive = 0;
        //    nNeckBigReceive = 0;
        //    nNeckSmallReceive = 0;
        //    nPlacket_2Receive = 0;
        //    nPiping_2Receive = 0;
        //    nHood_2Receive = 0;
        //    nHoodReceive = 0;
        //    nHoodRibReceive = 0;
        //    nPocketReceive = 0;
        //    nPocket_2Receive = 0;
        //    nPocketRib_2Receive = 0;
        //    nPocketBeg_2Receive = 0;
        //    nShoulderPatch_2Receive = 0;
        //    nElbowPatch_2Receive = 0;
        //    nMoonReceive = 0;
        //    nBottomRibReceive = 0;
        //    nArmholeReceive = 0;
        //    nFashionReceive = 0;

        //}
        //private void InitializeStringType()
        //{
        //    sFullString = "";
        //    sFrontString = "";
        //    sBackString = "";
        //    sSleeve_2String = "";
        //    sFrontPart_2String = "";
        //    sBackPart_2String = "";
        //    sNeckString = "";
        //    sNeckBigString = "";
        //    sNeckSmallString = "";
        //    sPlacket_2String = "";
        //    sPiping_2String = "";
        //    sHood_2String = "";
        //    sHoodString = "";
        //    sHoodRibString = "";
        //    sPocketString = "";
        //    sPocket_2String = "";
        //    sPocketRib_2String = "";
        //    sPocketBeg_2String = "";
        //    sShoulderPatch_2String = "";
        //    sElbowPatch_2String = "";
        //    sMoonString = "";
        //    sBottomRibString = "";
        //    sArmholeString = "";
        //    sFashionString = "";
        //    sStyleWiseString = "";
        //}


    }

}
