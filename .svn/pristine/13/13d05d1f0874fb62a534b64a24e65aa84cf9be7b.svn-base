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
    public class rptBlockMacineWiseReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        BlockMachineMappingReport _oBlockMachineMappingReport = new BlockMachineMappingReport();
        List<BlockMachineMappingReport> _oBlockMachineMappingReports = new List<BlockMachineMappingReport>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(BlockMachineMappingReport oBlockMachineMappingReport)
        {
            _oBlockMachineMappingReport = oBlockMachineMappingReport;
            _oBlockMachineMappingReports = oBlockMachineMappingReport.BlockMachineMappingReports;
            _oCompany = oBlockMachineMappingReport.Company;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 60f, 50f, 120f, 40f, 40f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Arial Black", 11f, iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("BLOCK WISE PRODUCTION REPORT ", _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("DATE : FROM " + _oBlockMachineMappingReport.ErrorMessage.Split(',')[0] + " TO " + _oBlockMachineMappingReport.ErrorMessage.Split(',')[1], _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 9;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion
        #region Report Body
        private void PrintHaedRow(BlockMachineMappingReport oBMM)
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("BLOCK " + oBMM.BlockName, _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Department : " + oBMM.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supervisor : " + oBMM.SupervisorName, _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Body Part", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color And Size", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Issue Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Received Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oBlockMachineMappingReports = _oBlockMachineMappingReports.OrderBy(x => x.BlockName).ToList();
            while (_oBlockMachineMappingReports.Count > 0)
            {
                List<BlockMachineMappingReport> oTempBlockMachineMappingReports = new List<BlockMachineMappingReport>();
                List<BlockMachineMappingReport> oTempMMRs = new List<BlockMachineMappingReport>();
                oTempBlockMachineMappingReports = _oBlockMachineMappingReports.Where(x => x.BlockName == _oBlockMachineMappingReports[0].BlockName).OrderBy(x => x.StyleNo).ToList();
                oTempMMRs.AddRange(oTempBlockMachineMappingReports);
                //List<string> BlockNames = new List<string>();
                //BlockNames = oTempBlockMachineMappingReports.Select(x => x.BlockName).ToList();
                //List<BlockMachineMappingReportReceiveDetail> oBlockMachineMappingReportReceiveDetails = new List<BlockMachineMappingReportReceiveDetail>();
                //oBlockMachineMappingReportReceiveDetails = (from oEPRDetail in _oBlockMachineMappingReportReceiveDetails
                //                                     where EPSIDs.Contains(oEPRDetail.EPSID)
                //                                     orderby oEPRDetail.StyleNo
                //                                     select oEPRDetail).ToList();
                GarmentPartCount(oTempBlockMachineMappingReports);
                PrintBlockMachine(oTempMMRs);
                _oBlockMachineMappingReports.RemoveAll(x => x.BlockName == oTempMMRs[0].BlockName);
            }

        }

        public void PrintBlockMachine(List<BlockMachineMappingReport> oBlockMachines)
        {

            PrintHaedRow(oBlockMachines[0]);
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            foreach (BlockMachineMappingReport oBMM in oBlockMachines)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oBMM.StyleNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oBMM.GPName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oBMM.SizeAndColor, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oBMM.IssueQty.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oBMM.RcvQty.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nBlockWiseTotalIssue.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nBlockWiseTotalReceive.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(BPartSting, _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion

        string BPartSting = "";
        double nBlockWiseTotalIssue = 0;
        double nBlockWiseTotalReceive = 0;

        private void GarmentPartCount(List<BlockMachineMappingReport> oBMMRs)
        {

            nBlockWiseTotalIssue = 0;
            nBlockWiseTotalReceive = 0;
            while (oBMMRs.Count > 0)
            {
                List<BlockMachineMappingReport> oTempBMMRs = new List<BlockMachineMappingReport>();
                oTempBMMRs = oBMMRs.Where(x => x.GarmentPart == oBMMRs[0].GarmentPart).ToList();
                double nIQty = 0;
                double nRQty = 0;
                BPartSting = "";
                foreach (BlockMachineMappingReport oBlockMachineMappingReport in oBMMRs)
                {
                    nIQty = nIQty + oBlockMachineMappingReport.IssueQty;
                    nRQty = nRQty + oBlockMachineMappingReport.RcvQty;
                    nBlockWiseTotalIssue += oBlockMachineMappingReport.IssueQty;
                    nBlockWiseTotalReceive += oBlockMachineMappingReport.RcvQty;
                }

                BPartSting = BPartSting + "[" + oTempBMMRs[0].GPName + ":T/I-" + nIQty + ",T/R-" + nRQty + "] ";
                oBMMRs.RemoveAll(x => x.GarmentPart == oTempBMMRs[0].GarmentPart);
            }




            //if (oItem.GarmentPart == EnumGarmentPart.Full)
            //{

            //    nFullTIssue += oItem.IssueQty;
            //    nFullTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Front)
            //{

            //    nFrontTIssue += oItem.IssueQty;
            //    nFrontTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Back)
            //{
            //    nBackTIssue += oItem.IssueQty;
            //    nBackTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Sleeve_2)
            //{

            //    nSleeve_2TIssue += oItem.IssueQty;
            //    nSleeve_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.FrontPart_2)
            //{

            //    nFrontPart_2TIssue += oItem.IssueQty;
            //    nFrontPart_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.BackPart_2)
            //{

            //    nBackPart_2TIssue += oItem.IssueQty;
            //    nBackPart_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Neck)
            //{

            //    nNeckTIssue += oItem.IssueQty;
            //    nNeckTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.NeckBig)
            //{

            //    nNeckBigTIssue += oItem.IssueQty;
            //    nNeckSmallTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.NeckSmall)
            //{

            //    nNeckSmallTIssue += oItem.IssueQty;
            //    nNeckSmallTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Placket_2)
            //{

            //    nPlacket_2TIssue += oItem.IssueQty;
            //    nPlacket_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Piping_2)
            //{

            //    nPiping_2TIssue += oItem.IssueQty;
            //    nPiping_2TReceive += oItem.RcvQty;
            //}

            //else if (oItem.GarmentPart == EnumGarmentPart.Hood_2)
            //{

            //    nHood_2TIssue += oItem.IssueQty;
            //    nHood_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Hood)
            //{

            //    nHoodTIssue += oItem.IssueQty;
            //    nHoodTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.HoodRib)
            //{

            //    nHoodRibTIssue += oItem.IssueQty;
            //    nHoodRibTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Pocket)
            //{

            //    nPocketTIssue += oItem.IssueQty;
            //    nPocketTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Pocket_2)
            //{

            //    nPocket_2TIssue += oItem.IssueQty;
            //    nPocket_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.PocketRib_2)
            //{

            //    nPocketRib_2TIssue += oItem.IssueQty;
            //    nPocketRib_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.PocketBeg_2)
            //{

            //    nPocketBeg_2TIssue += oItem.IssueQty;
            //    nPocketBeg_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.ShoulderPatch_2)
            //{

            //    nShoulderPatch_2TIssue += oItem.IssueQty;
            //    nShoulderPatch_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.ElbowPatch_2)
            //{

            //    nElbowPatch_2TIssue += oItem.IssueQty;
            //    nElbowPatch_2TReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Moon)
            //{

            //    nMoonTIssue += oItem.IssueQty;
            //    nMoonTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.BottomRib)
            //{

            //    nBottomRibTIssue += oItem.IssueQty;
            //    nBottomRibTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Armhole)
            //{

            //    nArmholeTIssue += oItem.IssueQty;
            //    nArmholeTReceive += oItem.RcvQty;
            //}
            //else if (oItem.GarmentPart == EnumGarmentPart.Fashion)
            //{

            //    nFashionTIssue += oItem.IssueQty;
            //    nFashionTReceive += oItem.RcvQty;
            //}
            //nBlockWiseTotalIssue += oItem.IssueQty;
            //nBlockWiseTotalReceive += oItem.RcvQty;

        }
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
        //    string sStyleWiseString = "";

        //    if (nFullTIssue != 0)
        //    {
        //        sFullString = "[Full : T/I-" + nFullTIssue + ",T/R-" + nFullTReceive + "]";

        //    }
        //    if (nFrontTIssue != 0)
        //    {
        //        sFrontString = "[Front : T/I-" + nFrontTIssue + ",T/R-" + nFrontTReceive + "]";

        //    }

        //    if (nBackTIssue != 0)
        //    {
        //        sBackString = "[Back : T/R-" + nBackTReceive + "]";

        //    }
        //    if (nSleeve_2TIssue != 0)
        //    {
        //        sSleeve_2String = "[Sleeve_2 : T/I-" + nSleeve_2TIssue + ",T/R-" + nSleeve_2TReceive + "]";

        //    }
        //    if (nFrontPart_2TIssue != 0)
        //    {
        //        sFrontPart_2String = " [FrontPart_2 : T/I-" + nSleeve_2TIssue + ",T/R-" + nSleeve_2TReceive + "]";

        //    }
        //    if (nBackPart_2TIssue != 0)
        //    {
        //        sBackPart_2String = " [BackPart_2 : T/I-" + nBackPart_2TIssue + ",T/R-" + nBackPart_2TReceive + "]";

        //    }
        //    else if (nNeckTIssue != 0)
        //    {
        //        sNeckString = " [Neck : T/I-" + nNeckTIssue + ",T/R-" + nNeckTReceive + "]";

        //    }
        //    if (nNeckBigTIssue != 0)
        //    {
        //        sNeckBigString = " [NeckBig : T/I-" + nNeckBigTIssue + ",T/R-" + nNeckBigTReceive + "]";

        //    }
        //    if (nNeckSmallTIssue != 0)
        //    {
        //        sNeckSmallString = " [NeckBig : T/I-" + nNeckSmallTIssue + ",T/R-" + nNeckSmallTReceive + "]";

        //    }
        //    if (nPlacket_2TIssue != 0)
        //    {
        //        sPlacket_2String = " [Placket_2 : T/I-" + nPlacket_2TIssue + ",T/R-" + nPlacket_2TReceive + "]";

        //    }
        //    if (nPiping_2TIssue != 0)
        //    {
        //        sPiping_2String = " [Piping_2 :  T/I-" + nPiping_2TIssue + ",T/R-" + nPiping_2TReceive + "]";

        //    }

        //    if (nHood_2TIssue != 0)
        //    {
        //        sHood_2String = " [Hood_2 :  T/I-" + nHood_2TIssue + ",T/R-" + nHood_2TReceive + "]";

        //    }
        //    if (nHoodTIssue != 0)
        //    {
        //        sHood_2String = " [Hood : T/I-" + nHoodTIssue + ",T/R-" + nHoodTReceive + "]";

        //    }
        //    if (nHoodRibTIssue != 0)
        //    {
        //        sHoodRibString = " [HoodRib : T/I-" + nHoodRibTIssue + ",T/R-" + nHoodRibTReceive + "]";

        //    }
        //    if (nPocketTIssue != 0)
        //    {
        //        sPocketString = " [Pocket : T/I-" + nPocketTIssue + ",T/R-" + nPocketTReceive + "]";

        //    }
        //    if (nPocket_2TIssue != 0)
        //    {
        //        sPocket_2String = " [Pocket_2 : T/I-" + nPocket_2TIssue + ",T/R-" + nPocket_2TReceive + "]";

        //    }
        //    if (nPocketRib_2TIssue != 0)
        //    {
        //        sPocketRib_2String = " [PocketRib_2 : T/I-" + nPocketRib_2TIssue + ",T/R-" + nPocketRib_2TReceive + "]";

        //    }
        //    if (nPocketBeg_2TIssue != 0)
        //    {
        //        sPocketBeg_2String = " [PocketBeg_2 : T/I-" + nPocketBeg_2TIssue + ",T/R-" + nPocketBeg_2TReceive + "]";

        //    }
        //    if (nShoulderPatch_2TIssue != 0)
        //    {
        //        sShoulderPatch_2String = " [ShoulderPatch_2 : T/I-" + nShoulderPatch_2TIssue + ",T/R-" + nShoulderPatch_2TReceive + "]";

        //    }
        //    if (nElbowPatch_2TIssue != 0)
        //    {
        //        sElbowPatch_2String = " [ElbowPatch_2 : T/I-" + nElbowPatch_2TIssue + ",T/R-" + nElbowPatch_2TReceive + "]";

        //    }
        //    if (nMoonTIssue != 0)
        //    {
        //        sMoonString = " [Moon : T/I-" + nMoonTIssue + ",T/R-" + nMoonTReceive + "]";

        //    }
        //    if (nBottomRibTIssue != 0)
        //    {
        //        sBottomRibString = " [BottomRib : T/I-" + nBottomRibTIssue + "T/R-" + nBottomRibTReceive + "]";

        //    }
        //    if (nArmholeTIssue != 0)
        //    {
        //        sArmholeString = " [Armhole : T/I-" + nArmholeTIssue + ",T/R-" + nArmholeTReceive + "]";

        //    }
        //    if (nFashionTIssue != 0)
        //    {
        //        sFashionString = " [Fashion : T/I-" + nFashionTIssue + ",T/R-" + nFashionTReceive + "]";

        //    }

        //    sStyleWiseString = sFullString + sFrontString + sBackString + sSleeve_2String + sFrontPart_2String + sBackPart_2String + sNeckString + sNeckBigString + sNeckSmallString + sPlacket_2String + sPiping_2String + sHood_2String + sHoodString + sHoodRibString + sPocketString + sPocket_2String + sPocketRib_2String + sPocketBeg_2String + sShoulderPatch_2String + sElbowPatch_2String + sMoonString + sBottomRibString + sArmholeString + sFashionString;
        //    return sStyleWiseString;
        //}
        //private void Initialize()
        //{

        //    nFullTIssue = 0;
        //    nFullTReceive = 0;
        //    nFrontTIssue = 0;
        //    nFrontTReceive = 0;
        //    nBackTIssue = 0;
        //    nBackTReceive = 0;
        //    nSleeve_2TIssue = 0;
        //    nSleeve_2TReceive = 0;
        //    nFrontPart_2TIssue = 0;
        //    nFrontPart_2TReceive = 0;
        //    nBackPart_2TIssue = 0;
        //    nBackPart_2TReceive = 0;
        //    nNeckTIssue = 0;
        //    nNeckTReceive = 0;
        //    nNeckBigTIssue = 0;
        //    nNeckBigTReceive = 0;
        //    nNeckSmallTIssue = 0;
        //    nNeckSmallTReceive = 0;
        //    nPlacket_2TIssue = 0;
        //    nPlacket_2TReceive = 0;
        //    nPiping_2TIssue = 0;
        //    nPiping_2TReceive = 0;
        //    nHood_2TIssue = 0;
        //    nHood_2TReceive = 0;
        //    nHoodTIssue = 0;
        //    nHoodTReceive = 0;
        //    nHoodRibTIssue = 0;
        //    nHoodRibTReceive = 0;
        //    nPocketTIssue = 0;
        //    nPocketTReceive = 0;
        //    nPocket_2TIssue = 0;
        //    nPocket_2TReceive = 0;
        //    nPocketRib_2TIssue = 0;
        //    nPocketRib_2TReceive = 0;
        //    nPocketBeg_2TIssue = 0;
        //    nPocketBeg_2TReceive = 0;
        //    nShoulderPatch_2TIssue = 0;
        //    nShoulderPatch_2TReceive = 0;
        //    nElbowPatch_2TIssue = 0;
        //    nElbowPatch_2TReceive = 0;
        //    nMoonTIssue = 0;
        //    nMoonTReceive = 0;
        //    nBottomRibTIssue = 0;
        //    nBottomRibTReceive = 0;
        //    nArmholeTIssue = 0;
        //    nArmholeTReceive = 0;
        //    nFashionTIssue = 0;
        //    nFashionTReceive = 0;

        //    nBlockWiseTotalIssue = 0;
        //    nBlockWiseTotalReceive = 0;

        //}
    }

}
