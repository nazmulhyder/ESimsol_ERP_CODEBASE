using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.BusinessObjects.ReportingObject;
using System.Collections;

namespace ESimSol.Reports
{
    public class rptFabricSpecification
    {
        #region Declaration
        private int _nColumn = 1;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
       
        private iTextSharp.text.Font _oFontStyleBold;
        private PdfPTable _oPdfPTable = new PdfPTable(1);
        private PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        FabricSCReport _oFSR = new FabricSCReport();
        FabricSalesContract _oFSC = new FabricSalesContract();
        FabricExecutionOrderSpecification _oFEOS = new FabricExecutionOrderSpecification();
        List<FabricDeliverySchedule> _oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
        List<FabricSalesContractNote> _oFabricSalesContractNotes = new List<FabricSalesContractNote>();
        List<FabricSpecificationNote> _oFabricSpecificationNotes = new List<FabricSpecificationNote>();
        List<FabricDyeingRecipe> _oFabricDyeingRecipes = new List<FabricDyeingRecipe>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<FabricDispo> _oFabricDispos = new List<FabricDispo>();
        private Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();
        Fabric _oFabric = new Fabric();
        double _nTotalDSQty = 0;
        float _nUsagesHeight = 0;
        int _nCount = 0;
        #endregion
        public byte[] PrepareReport(FabricExecutionOrderSpecification oFEOS, FabricSCReport oFSR, FabricSalesContract oFSC, Fabric oFabric, Company oCompany, List<FabricDeliverySchedule> oFebricDeliverySchedules, List<FabricSalesContractNote> oFabricSalesContractNotes, List<FabricSpecificationNote> oFabricSpecificationNotes, List<FabricDispo> oFabricDispos, List<FabricDyeingRecipe> oFabricDyeingRecipes, Contractor oContractor)
        {
            _oContractor = oContractor;
            _oFEOS = oFEOS;
            _oFSR = oFSR;
            _oFSC = oFSC;
            _oFabricDispos = oFabricDispos;
            _oFebricDeliverySchedules = oFebricDeliverySchedules;
            _oFabricSalesContractNotes = oFabricSalesContractNotes;
            _oFabricSpecificationNotes = oFabricSpecificationNotes;
            _oFabricDyeingRecipes = oFabricDyeingRecipes;
            _oCompany = oCompany;
            _oFabric = oFabric;
            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PageEventHandler.signatures = new List<string> { "Prepared By", "Checked By", "Approve By" };
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 545f });

            #endregion

            this.PrintHeaderTwo();
            this.PrintWaterMark(10f, 10f, 10f, 10f);
            this.PrintBody();
            this.Print_Instruction();
            if (_oFEOS.ProdtionTypeInt == (int)(EnumDispoProType.None) || _oFEOS.ProdtionTypeInt == (int)(EnumDispoProType.General) || _oFEOS.ProdtionTypeInt == (int)(EnumDispoProType.ExcessFabric))
            {
                this.Print_DeliverySchedule();
            }
            this.PrintFooter_Default();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(PrintHeaderWithLogo());
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private PdfPTable PrintCompanyInfo()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 400f });
            PdfPCell oPdfPCell;
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.ExtraParagraphSpace = 0;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
            // if yarn Dyed
            if (_oFEOS.IsYD)
            {
                oPdfPCell = new PdfPCell(new Phrase("Y/D FABRIC SPECIFICATION", _oFontStyle));
            }
            else { oPdfPCell = new PdfPCell(new Phrase("S/D FABRIC SPECIFICATION", _oFontStyle)); }
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable PrintHeaderWithLogo()
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 10f, 200f });
            PdfPCell oPdfPCell;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                oPdfPCell = new PdfPCell(_oImag);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell.Border = 0;
                oPdfPCell.FixedHeight = 35;
                oPdfPTable.AddCell(oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(PrintCompanyInfo());
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            if (_oCompany.CompanyLogo == null)
            {
                oPdfPCell.Colspan = 2;
            }
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        #endregion

        #region Report Header
        private void PrintHeaderTwo()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 260.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Rowspan = 2;
                _oPdfPCell.MinimumHeight = 25;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Rowspan = 2; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
            //if (_oFSR.ProcessType == 1)
            // if yarn Dyed
            if (_oFEOS.IsYD)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Y/D FABRIC SPECIFICATION", _oFontStyle));
            }
            else { _oPdfPCell = new PdfPCell(new Phrase("S/D FABRIC SPECIFICATION", _oFontStyle)); }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            if (_oFEOS.ProdtionTypeInt == (int)(EnumDispoProType.ExcessDyeingQty))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Excess Dyeing Qty" + ((_oFEOS.ReviseNo > 0) ? " Revise-" + _oFEOS.ReviseNo.ToString() : ""), _oFontStyle));
            }
            else if (_oFEOS.ProdtionTypeInt == (int)(EnumDispoProType.ExcessFabric))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Excess Fabric" + ((_oFEOS.ReviseNo > 0) ? " Revise-" + _oFEOS.ReviseNo.ToString() : ""), _oFontStyle));
            }
            else if (_oFEOS.ProdtionTypeInt == (int)(EnumDispoProType.OutSide))
            {
                _oPdfPCell = new PdfPCell(new Phrase("Out Side" + ((_oFEOS.ReviseNo > 0) ? " Revise-" + _oFEOS.ReviseNo.ToString() : ""), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase((_oFEOS.ReviseNo > 0) ? " Revise-" + _oFEOS.ReviseNo.ToString() : "", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD)));
             //   _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
             _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion

        #region Body
        
        private void PrintBody()
        {
            string sExcNo = "";
            string sCode = "";
            string sTemp= "";
           ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyle, true);

           bool _bIsReProduction = false;

            PdfPTable oPdfPTable;
            var fontBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            var fontNormal = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region Fabric Info
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 45f, 100f, 125f, 145f, 130f });

            #endregion


            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Line No: " + _oFSR.SLNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PO NO: " + _oFSR.SCNoFull, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? _oContractor.Name : "PO Recv Date: " + _oFSR.FabricReceiveDateStr), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count(Warp X Weft)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.WarpCount + "X" + _oFEOS.WeftCount, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();
             //_oFEOS.ProdtionType == true ? "ASAP" : ""

            sTemp = (string.IsNullOrEmpty(_oFSR.BuyerName) ? "" : _oFSR.BuyerName);
            sTemp = sTemp+(string.IsNullOrEmpty(sTemp) ? "" : ",");
            sTemp = sTemp+(string.IsNullOrEmpty(_oFSR.ContractorName) ? "" : _oFSR.ContractorName);
            if (_oFEOS.IsOutSide == true && _oFEOS.ContractorID>0)
            {
                sTemp = _oCompany.Name;
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer: " + sTemp, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Composition(Warp X Weft)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.ProductName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();
            #endregion

            #region Finished Fabric Detail
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finished Fabric Detail", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ?"":"PO Construction"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? "" : _oFSR.Construction), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            _bIsReProduction = false;
            if (_oFEOS.ProdtionTypeInt == 2 || _oFEOS.ProdtionTypeInt == 3)
            {
                _bIsReProduction = true;
            }
            if (_oFabricDispos.Count > 0)
            {
                sCode = _oFabricDispos.Where(x => x.IsReProduction == _bIsReProduction && x.BusinessUnitType == EnumBusinessUnitType.Finishing && x.IsYD == _oFEOS.IsYD).FirstOrDefault().Code;
            }
            if (!string.IsNullOrEmpty(_oFSR.ExeNo))
            {
                sExcNo = _oFSR.ExeNo;
                sExcNo = sExcNo.Remove(0, 1);
            }
            sExcNo = sCode + "" + sExcNo;

          
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No: " + sExcNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(_oFEOS.RefNo) ? "Ref: " + _oFSR.HLReference : "Ref: " + _oFEOS.RefNo) + (string.IsNullOrEmpty(_oFSR.Note) ? "" : " Note:" + _oFSR.Note), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ends X Picks(Finished)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? "" : _oFEOS.FinishEnd.ToString() + " X " + _oFEOS.FinishPick.ToString()), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            // if yarn Dyed
            if (_oFEOS.IsYD)
           // if (_oFSR.ProcessType == 1)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Type", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.FabricDesignName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
              
            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.WeftColor, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finished Fabric Width(inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable,((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? "" : Math.Round(_oFEOS.FinishWidth-1,2).ToString()+ "\xB''"+"/" +Math.Round(_oFEOS.FinishWidth,2).ToString()+ "\xB''"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Design", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.FabricWeaveName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Required Finished length(Meter)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            if (_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat_Round(_oFEOS.OrderQtyInMeter), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            }
            else
            {
                if (_oFSR.OrderType == (int)EnumFabricRequestType.Sample || _oFSR.OrderType == (int)EnumFabricRequestType.SampleFOC)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat_Round(_oFEOS.OrderQtyInMeter + _oFEOS.QtyExtraMet), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                }
                else
                {
                    if (_oFEOS.QtyExtraMet > 0)
                    { ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat_Round(_oFEOS.OrderQtyInMeter) + "+ " + Global.MillionFormat_Round(Math.Round(_oFEOS.QtyExtraMet, 2)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal); }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat_Round(_oFEOS.OrderQtyInMeter), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    }
                }
            }
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            if (_oFSR.IsPrint)
            {
                if (_oFSR.FinishTypeName.Contains("Ready for Print"))
                {
                    _oFSR.FinishTypeName = _oFSR.FinishTypeName;
                }
                else
                {
                    _oFSR.FinishTypeName = "Ready for Print Then " + _oFSR.FinishTypeName;
                }
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.FinishTypeName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Date(Finished)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            if (_oFEOS.ProdtionType == EnumDispoProType.ExcessFabric || _oFEOS.ProdtionType == EnumDispoProType.ExcessDyeingQty)
            { ESimSolItexSharp.SetCellValue(ref oPdfPTable, "ASAP", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal); }
            else
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? "" : _oFEOS.FinishedDateStr), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Greige Fabric Detail
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Fabric Detail", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();

            if (_oFEOS.ProdtionTypeInt != (int)(EnumDispoProType.ExcessDyeingQty))
            {

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                if (_oFEOS.ProdtionTypeInt == 2 || _oFEOS.ProdtionTypeInt == 3)
                {
                    _bIsReProduction = true;
                }
                if (_oFabricDispos.Count > 0)
                {
                    sCode = _oFabricDispos.Where(x => x.IsReProduction == _bIsReProduction && x.BusinessUnitType == EnumBusinessUnitType.Weaving && x.IsYD == _oFEOS.IsYD).FirstOrDefault().Code;
                }
                if (!string.IsNullOrEmpty(_oFSR.ExeNo))
                {
                    sExcNo = _oFSR.ExeNo;
                    sExcNo = sExcNo.Remove(0, 1);
                }
                sExcNo = sCode + "" + sExcNo;
                sTemp = "";
                if (!string.IsNullOrEmpty(_oFEOS.RefNote))
                {
                    sTemp = _oFEOS.RefNote;
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No: " + sExcNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req: " + _oFSR.FabricNo + " " + sTemp, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ends X Picks(Greige)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Ends + " X " + _oFEOS.Picks, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer Ref", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                string stemp = "";
                if (!string.IsNullOrEmpty(_oFSR.StyleNo)) { stemp = _oFSR.StyleNo; }
                if (!string.IsNullOrEmpty(stemp)) { stemp = stemp + ";"; }
                if (!string.IsNullOrEmpty(_oFSR.BuyerReference)) { stemp = stemp + "" + _oFSR.BuyerReference; }

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, stemp, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Width(inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                //  _oFEOS.Ends = _oFEOS.Reed * (_oFEOS.ReedWidth + (_oFEOS.ReedWidth * _oFEOS.Crimp / 100)) / _oFEOS.Ends;
                double nTotalEnds = 0;
                if (_oFEOS.IsTEndsAdd) { nTotalEnds = Math.Round(_oFEOS.TotalEnds + _oFEOS.TotalEndsAdd); } else { nTotalEnds = Math.Round(_oFEOS.TotalEnds - _oFEOS.TotalEndsAdd); }

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((nTotalEnds) / _oFEOS.Ends) + "\xB''", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Color", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Required Greige(Meter)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? "" : Global.MillionFormat_Round(_oFEOS.GreigeDemand)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Reed.ToString() + "/" + _oFEOS.Dent.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Loom Production(Mtr)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat_Round(_oFEOS.ReqLoomProduction), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed Width(inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFEOS.ReedWidth), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Warp Length(Mtr)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                if (_oFEOS.FSpcType == EnumFabricSpeType.SeerSucker)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Upper Beam: " + Global.MillionFormat_Round(_oFEOS.RequiredWarpLength) + "\nLowerBeam: " + Global.MillionFormat_Round(_oFEOS.ReqWarpLenLB), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat_Round(_oFEOS.RequiredWarpLength), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                }
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.ProdtionType == EnumDispoProType.ExcessDyeingQty || _oFEOS.ProdtionType == EnumDispoProType.ExcessDyeingQty) ? "ASAP" : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ground Ends", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ////  _oFEOS.GroundEnds = (_oFEOS.FinishEnd * _oFEOS.FinishWidth) - _oFEOS.SelvedgeEnds - _oFEOS.SelvedgeEndTwo;
                //  _oFEOS.GroundEnds = (_oFEOS.Reed * _oFEOS.ReedWidth)- _oFEOS.SelvedgeEnds - _oFEOS.SelvedgeEndTwo;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Math.Round(_oFEOS.GroundEnds, 0).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "R&D Comments", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.NoOfFrame, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Selvedge Ends", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (Math.Round(_oFEOS.SelvedgeEnds, 0)).ToString() + "+" + Math.Round(_oFEOS.SelvedgeEndTwo, 0).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Ends", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                sTemp = "";
                if (_oFEOS.FSpcType == EnumFabricSpeType.SeerSucker)
                {
                    sTemp = "UpperBeam=" + Math.Round(_oFEOS.TotalEndsUB).ToString() + "," + " LowerBeam=" + Math.Round(_oFEOS.TotalEndsLB).ToString();
                }
                if (_oFEOS.TotalEndsAdd > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.IsTEndsAdd == true ? (Math.Round(_oFEOS.TotalEnds + _oFEOS.TotalEndsAdd)).ToString() : (Math.Round(_oFEOS.TotalEnds - _oFEOS.TotalEndsAdd)).ToString() + "") + (string.IsNullOrEmpty(sTemp) ? "" : "\n=" + sTemp), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (string.IsNullOrEmpty(sTemp) ? "" : sTemp + "=") + (Math.Round(_oFEOS.TotalEnds)).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                }
                oPdfPTable.CompleteRow();
            }
            else
            {

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                if (_oFEOS.ProdtionTypeInt == 2 || _oFEOS.ProdtionTypeInt == 3)
                {
                    _bIsReProduction = true;
                }
                if (_oFabricDispos.Count > 0)
                {
                    sCode = _oFabricDispos.Where(x => x.IsReProduction == _bIsReProduction && x.BusinessUnitType == EnumBusinessUnitType.Weaving && x.IsYD == _oFEOS.IsYD).FirstOrDefault().Code;
                }
                if (!string.IsNullOrEmpty(_oFSR.ExeNo))
                {
                    sExcNo = _oFSR.ExeNo;
                    sExcNo = sExcNo.Remove(0, 1);
                }
                sExcNo = sCode + "" + sExcNo;
                sTemp = "";
                if (!string.IsNullOrEmpty(_oFEOS.RefNote))
                {
                    sTemp = _oFEOS.RefNote;
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No: " + sExcNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req: " + _oFSR.FabricNo + " " + sTemp, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ends X Picks(Greige)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer Ref", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                string stemp = "";
                if (!string.IsNullOrEmpty(_oFSR.StyleNo)) { stemp = _oFSR.StyleNo; }
                if (!string.IsNullOrEmpty(stemp)) { stemp = stemp + ";"; }
                if (!string.IsNullOrEmpty(_oFSR.BuyerReference)) { stemp = stemp + "" + _oFSR.BuyerReference; }

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, stemp, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Width(inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                //  _oFEOS.Ends = _oFEOS.Reed * (_oFEOS.ReedWidth + (_oFEOS.ReedWidth * _oFEOS.Crimp / 100)) / _oFEOS.Ends;
         
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Color", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Required Greige(Meter)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Reed.ToString() + "/" + _oFEOS.Dent.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Loom Production(Mtr)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed Width(inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFEOS.ReedWidth), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. Warp Length(Mtr)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
             
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFEOS.ProdtionType == EnumDispoProType.ExcessDyeingQty || _oFEOS.ProdtionType == EnumDispoProType.ExcessDyeingQty) ? "ASAP" : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ground Ends", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ////  _oFEOS.GroundEnds = (_oFEOS.FinishEnd * _oFEOS.FinishWidth) - _oFEOS.SelvedgeEnds - _oFEOS.SelvedgeEndTwo;
                //  _oFEOS.GroundEnds = (_oFEOS.Reed * _oFEOS.ReedWidth)- _oFEOS.SelvedgeEnds - _oFEOS.SelvedgeEndTwo;
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "R&D Comments", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.NoOfFrame, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Selvedge Ends", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Ends", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
          
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                
                oPdfPTable.CompleteRow();
            }
            #endregion

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #region Color Break Down
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Color Break Down", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontBold, true);

            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 45f, 100f, 125f, 75f, 75f, 65f, 65f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "ALD NO", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Quantity(kg)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Length(Mtr)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            // if yarn Dyed
            if (_oFEOS.IsYD)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Cones", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            }
            oPdfPTable.CompleteRow();

            if (_oFEOS.IsOutSide == true && _oFEOS.ContractorID> 0)
            {
                foreach (FabricExecutionOrderSpecificationDetail oitem in _oFEOS.FEOSDetails)
                {
                    if (_oFEOS.WarpWeftType == EnumWarpWeft.Warp)
                    {
                        if (oitem.IsWarp == false)
                        {
                            oitem.Qty = 0;
                            oitem.Length = 0;
                        }
                    }
                    if (_oFEOS.WarpWeftType == EnumWarpWeft.Weft)
                    {
                        if (oitem.IsWarp == true)
                        {
                            oitem.Qty = 0;
                            oitem.Length = 0;
                        }

                    }
                }

            }

            List<FabricExecutionOrderSpecificationDetail> oFEOSDsWarps = _oFEOS.FEOSDetails.Where(x => x.IsWarp == true).ToList();
            List<FabricExecutionOrderSpecificationDetail> oFEOSDsWefts = _oFEOS.FEOSDetails.Where(x => x.IsWarp == false).ToList();

            oFEOSDsWarps = oFEOSDsWarps.OrderBy(x => x.SLNo).ToList();
            oFEOSDsWefts = oFEOSDsWefts.OrderBy(x => x.SLNo).ToList();

            double nQty = 0;
            double nQty_Al = 0;
            int nTotalend = 0;
            int nRowSpan_Twist = 0;
            int nTwistGroup = 0;
            string sTwist = "Grindle";
            #region Warp
            if(oFEOSDsWarps.Any())
            {
                nTotalend = oFEOSDsWarps.Sum(x => x.EndsCount);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp", oFEOSDsWarps.Count()+1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                //_oFEOS.NoOfSection = Math.Round(Math.Ceiling(_oFEOS.NoOfSection));
                foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDsWarps)
                {
                    //nWarpWeightInkg = (_oFEOS.RepeatSection * oItem.EndsCount * _oFEOS.NoOfSection * Math.Round(Math.Ceiling(_oFEOS.RequiredWarpLength),0)* 0.0005905 / (oItem.Value - oItem.ValueMin)) / (((100 - _oFEOS.AllowancePercent) / 100));
                    //nLength = (Math.Round(nWarpWeightInkg, 2) * 2.2046 * 840 * oItem.Value * 0.9144) / (_oFEOS.RepeatSection * oItem.EndsCount);
                    //oItem.Qty = Math.Round(nWarpWeightInkg,2);
                    //oItem.Length = Math.Round(nLength,2);
                    if (_oFEOS.IsSepBeam && oItem.Length > 0) { oItem.Length = oItem.Length / 2; oItem.EndsCount = oItem.EndsCount * 2; }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductShortName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    if (_oFEOS.FSpcType==EnumFabricSpeType.SeerSucker && oItem.BeamType != EnumBeamType.None)
                    {
                        sTemp = oItem.BeamType.ToString();
                    }
                    //if (!string.IsNullOrEmpty(oItem.PantonNo)) { oItem.ColorName = oItem.ColorName + "(" + oItem.PantonNo + ")"; }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorName + "" + (string.IsNullOrEmpty(oItem.PantonNo) ? "" : "(" + oItem.PantonNo+")") + " " + (string.IsNullOrEmpty(sTemp) ? "" : "" + sTemp), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                    {
                           nRowSpan_Twist = oFEOSDsWarps.Where(P => P.TwistedGroup == oItem.TwistedGroup && P.TwistedGroup>0).ToList().Count;
                           if (oItem.TwistedGroup > 0 && nTwistGroup != oItem.TwistedGroup)
                           {
                               ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                               //oItem.Qty = 0;
                           }
                           //else { oItem.Qty = 0; }
                           if (oItem.TwistedGroup <= 0)
                           {
                               ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                           }
                           sTwist = "Injected Slub";
                    }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    }

                    if (_oFEOS.IsYD)
                    {
                        nRowSpan_Twist = oFEOSDsWarps.Where(P => P.TwistedGroup == oItem.TwistedGroup && P.TwistedGroup>0).ToList().Count;
                        if (oItem.TwistedGroup > 0 && nTwistGroup != oItem.TwistedGroup)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTwist, (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (Math.Round(_oFEOS.RepeatSection * oItem.EndsCount, 0)).ToString(), (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                        }
                        if (oItem.TwistedGroup<=0)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Length) :""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            if (_oFEOS.FSpcType == EnumFabricSpeType.SeerSucker)
                            {
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.NoOfSection<=0)?"":(Math.Round(oItem.TotalEndActual / _oFEOS.NoOfSection, 0)).ToString()), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            }
                            else
                            {
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? (Math.Round(_oFEOS.RepeatSection * oItem.EndsCount, 0)).ToString() : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            }
                        }
                    }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.YarnType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    }
                    oPdfPTable.CompleteRow();
                    nTwistGroup = oItem.TwistedGroup;
                }
            // nTotalend=  oFEOSDsWarps.Where(x => (x.Pro == oItem.Isw)).ToList().Select(k=>k.EndsCount).Sum();
               //  nTotalend = oFEOSDsWarps.Sum(x => x.EndsCount);
                 if (oFEOSDsWarps.Count <= 2 && nTotalend <= 4 && _oFEOS.RequiredWarpLength >= 4000)
                 {
                     sTemp = "Beam:";
                                    }
                 else { sTemp = "Section:"; }
                 if (_oFEOS.IsSepBeam) { sTemp = sTemp + "(" + (_oFEOS.NoOfSection / 2).ToString() + "+" + (_oFEOS.NoOfSection / 2).ToString() + ")"; }
                 if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                 {
                          nQty_Al = oFEOSDsWarps.Where(x =>  x.TwistedGroup <= 0).Select(o => o.Qty).Sum();
                          var oTemp = oFEOSDsWarps.Where(x => x.TwistedGroup> 0).GroupBy(x => new { x.TwistedGroup, x.Qty }, (key, grp) => new FabricExecutionOrderSpecificationDetail { TwistedGroup = key.TwistedGroup, Qty = key.Qty }).ToList();
                          nQty_Al = nQty_Al + oTemp.Select(o => o.Qty).Sum();
                 }
                 else
                 {
                     nQty_Al = oFEOSDsWarps.Select(o => o.Qty).Sum();
                 }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable,((nQty_Al > 0) ? Global.MillionFormat(nQty_Al) : "") , 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                if (_oFEOS.IsYD)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTemp + " " + ((_oFEOS.IsSepBeam)?"":(_oFEOS.NoOfSection).ToString()), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                }
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Weft
            if (oFEOSDsWefts.Any())
            {

               ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weft", oFEOSDsWefts.Count() + 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
               nTotalend = oFEOSDsWefts.Sum(x => x.EndsCount);
               nTwistGroup = 0;
               foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDsWefts)
                //foreach (var oItem in oFEOSDsWefts)
                {
                     ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductShortName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                     sTemp = "";
                     if (_oFEOS.FSpcType == EnumFabricSpeType.SeerSucker && oItem.BeamType != EnumBeamType.None)
                     {
                         sTemp = oItem.BeamType.ToString();
                     }
                    // if (!string.IsNullOrEmpty(oItem.PantonNo)) { oItem.ColorName = oItem.ColorName + "(" + oItem.PantonNo + ")"; }
                     ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorName + "" + (string.IsNullOrEmpty(oItem.PantonNo) ? "" : "(" + oItem.PantonNo + ")")  +" " + (string.IsNullOrEmpty(sTemp) ? "" : "\n" + sTemp), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                     ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                     //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);

                     if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                     {
                         sTwist = "Injected Slub";
                         nRowSpan_Twist = oFEOSDsWefts.Where(P => P.TwistedGroup == oItem.TwistedGroup && P.TwistedGroup > 0).ToList().Count;
                         if (oItem.TwistedGroup > 0 && nTwistGroup != oItem.TwistedGroup)
                         {
                             ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                             //oItem.Qty = 0; 
                         }
                         //else { oItem.Qty = 0; }
                         if (oItem.TwistedGroup <= 0)
                         {
                             ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                         }
                     }
                     else
                     {
                         ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                     }
                     // if yarn Dyed
                     if (_oFEOS.IsYD)
                     //if (_oFSR.ProcessType == 1)
                     {
                         nRowSpan_Twist = oFEOSDsWefts.Where(P => P.TwistedGroup == oItem.TwistedGroup && P.TwistedGroup > 0).ToList().Count;
                         if (oItem.TwistedGroup > 0 && nTwistGroup != oItem.TwistedGroup)
                         {
                             ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTwist, (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                             ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                         }
                         if (oItem.TwistedGroup <= 0)
                         {
                             ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                             ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                         }
                     }
                     else
                     {
                         ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                         ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.YarnType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                     }
                     oPdfPTable.CompleteRow();
                     nTwistGroup = oItem.TwistedGroup;
                }
               if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
               {
                   nQty_Al = oFEOSDsWefts.Where(x => x.TwistedGroup <= 0).Select(o => o.Qty).Sum();
                   var oTemp = oFEOSDsWefts.Where(x => x.TwistedGroup > 0).GroupBy(x => new { x.TwistedGroup, x.Qty }, (key, grp) => new FabricExecutionOrderSpecificationDetail { TwistedGroup = key.TwistedGroup, Qty = key.Qty }).ToList();
                   nQty_Al = nQty_Al + oTemp.Select(o => o.Qty).Sum();
               }
               else
               {
                   nQty_Al = oFEOSDsWefts.Select(o => o.Qty).Sum();
               }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weft Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty_Al > 0) ? Global.MillionFormat(nQty_Al) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                oPdfPTable.CompleteRow();
            }
            #endregion

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            if (_oFEOS.IsYD) // if yarn Dyed
          //  if (_oFSR.ProcessType == 1) // if yarn Dyed
            {
                #region Dyed Yarn Details
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 45f, 50f, 125f, 75f, 75f, 65f, 115f });

                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Dyed Yarn Details", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontBold, true);
                _bIsReProduction = false;
                if (_oFEOS.ProdtionTypeInt == 2 || _oFEOS.ProdtionTypeInt == 3)
                {
                    _bIsReProduction = true;
                }
                if (_oFabricDispos.Count > 0)
                {
                    sCode = _oFabricDispos.Where(x => x.IsReProduction == _bIsReProduction && x.BusinessUnitType == EnumBusinessUnitType.Dyeing && x.IsYD == true).FirstOrDefault().Code;
                }
                if (!string.IsNullOrEmpty(_oFSR.ExeNo))
                {
                    sExcNo = _oFSR.ExeNo;
                    sExcNo = sExcNo.Remove(0, 1);
                }
                sExcNo = sCode + "" + sExcNo;
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 10f, fontNormal, true);
                //if (_oFSR.ExeNo.Contains("W"))
                //{
                //    sExcNo = _oFSR.ExeNo;
                //    sExcNo = sExcNo.Replace("W", "Y");
                //}
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Dispo No: " + sExcNo + ",  Delivery Date: ", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal, true);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Breakdown of Yarn For Dyeing", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal, true);
                //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 10f, fontNormal, true);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "ALD No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Yarn(kg)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dyed Yarn(Kg)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                oPdfPTable.CompleteRow();



                List<FabricExecutionOrderSpecificationDetail> oFEOSDs = new List<FabricExecutionOrderSpecificationDetail>();
                oFEOSDs = _oFEOS.FEOSDetails.GroupBy(x => new { x.ProductName, x.ProductShortName, x.ProductID, x.LabdipDetailID, x.ColorName, x.PantonNo, x.ColorNo, x.Value, x.Allowance, x.BatchNo, x.YarnType }, (key, grp) =>
                               new FabricExecutionOrderSpecificationDetail
                               {
                                   IsWarp = _oFEOS.FEOSDetails.Where(x => x.LabdipDetailID == key.LabdipDetailID && x.ColorNo == key.ColorNo && x.Value == key.Value && x.ColorName == key.ColorName && x.ProductID == key.ProductID).FirstOrDefault().IsWarp,
                                   SLNo = _oFEOS.FEOSDetails.Where(x => x.LabdipDetailID == key.LabdipDetailID && x.ColorNo == key.ColorNo && x.Value == key.Value && x.ColorName == key.ColorName && x.ProductID == key.ProductID).FirstOrDefault().SLNo,
                                   IsYarnExist = _oFEOS.FEOSDetails.Where(x => x.LabdipDetailID == key.LabdipDetailID && x.ColorNo == key.ColorNo && x.Value == key.Value && x.ColorName == key.ColorName && x.ProductID == key.ProductID).FirstOrDefault().IsYarnExist,
                                   ProductID = key.ProductID,
                                   ProductName = key.ProductName,
                                   ColorNo = key.ColorNo,
                                   ColorName = key.ColorName,
                                   PantonNo = key.PantonNo,
                                   LabdipDetailID = key.LabdipDetailID,
                                   ProductShortName = key.ProductShortName,
                                   YarnType = key.YarnType,
                                   Value = key.Value,
                                   Allowance = key.Allowance,
                                   Qty = grp.Sum(p => p.Qty),
                                   Length = grp.Sum(p => p.Length),
                                   BatchNo = key.BatchNo,
                               }).ToList();


                oFEOSDs = oFEOSDs.OrderBy(x => x.SLNo).ToList();
                if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                {
                    foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDs)
                    {
                        oItem.TwistedGroup = _oFEOS.FEOSDetails.Where(x => x.LabdipDetailID == oItem.LabdipDetailID && x.ColorNo == oItem.ColorNo && x.Value == oItem.Value && x.ColorName == oItem.ColorName && x.ProductID == oItem.ProductID).FirstOrDefault().TwistedGroupInt;
                        // oItem.TwistedGroupInt = _oFEOS.FEOSDetails.Where(x => x.LabdipDetailID == oItem.LabdipDetailID && x.ColorNo == oItem.ColorNo && x.Value == oItem.Value && x.ColorName == oItem.ColorName).FirstOrDefault().TwistedGroupInt;
                    }
                }
                nTwistGroup = 0;
                nQty_Al = 0;
                nQty = 0;
                nRowSpan_Twist = 0;
                string sPreviousItem = string.Empty;
                foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOSDs)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BatchNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductShortName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    if (!string.IsNullOrEmpty(oItem.PantonNo)) { oItem.PantonNo = oItem.ColorName + "(" + oItem.PantonNo + ")"; } else { oItem.PantonNo = oItem.ColorName; }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.PantonNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ColorNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    nQty = (oItem.Qty / ((100 - oItem.Allowance) / 100)); if ( nQty>0 && nQty < 1 ) { nQty = 1; }
                    if (oItem.IsYarnExist) { nQty = 0; }
                    if (_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0)
                    {
                        nQty = 0;
                    }

                    if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                    {
                        nRowSpan_Twist = oFEOSDs.Where(P => P.TwistedGroup == oItem.TwistedGroup && P.TwistedGroup > 0).ToList().Count;
                        if (oItem.TwistedGroup > 0 && nTwistGroup != oItem.TwistedGroup )
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty > 0) ? Global.MillionFormat(nQty) : ""), (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), (nRowSpan_Twist > 0) ? nRowSpan_Twist : 1, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            nQty = 0;
                           // oItem.Qty = 0;
                        }
                        if (oItem.TwistedGroup==0 )
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty > 0) ? Global.MillionFormat(nQty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                        }
                      
                    }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty > 0) ? Global.MillionFormat(nQty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.Qty > 0) ? Global.MillionFormat(oItem.Qty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    }

                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty > 0) ? Global.MillionFormat(nQty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Qty), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.YarnType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
                    nQty_Al = nQty_Al + nQty;// oItem.Qty / ((100 - oItem.Allowance) / 100);
                    nTwistGroup = oItem.TwistedGroup;
                    oPdfPTable.CompleteRow();
                }
                //if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                //{
                //    nQty = oFEOSDs.Where(x=>x.TwistedGroupInt>0).Select(o => o.Qty).Sum();
                //}
                //else
                //{
                    nQty = oFEOSDs.Select(o => o.Qty).Sum();
                //}
                    if (_oFEOS.FSpcType == EnumFabricSpeType.InjectedSlub)
                    {
                        var oFEOSDs2 = oFEOSDs.GroupBy(x => new { x.ProductName, x.ProductShortName, x.ProductID,  x.Value, x.Allowance,  x.YarnType, x.TwistedGroup, x.IsYarnExist }, (key, grp) =>
                                   new 
                                   {
                                       ProductID = key.ProductID,
                                       Value = key.Value,
                                       ProductName = key.ProductName,
                                       Allowance = key.Allowance,
                                       Qty = grp.FirstOrDefault().Qty,
                                       IsYarnExist = key.IsYarnExist,
                                       TwistedGroup = key.TwistedGroup
                                   }).ToList();

                    nQty = oFEOSDs2.Where(x => x.IsYarnExist == false).Select(o => o.Qty).Sum();
                    nQty_Al = oFEOSDs2.Where(x => x.IsYarnExist == false).Select(o => o.Qty / ((100 - o.Allowance) / 100)).Sum();
                      
                    }

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty_Al > 0) ? Global.MillionFormat(nQty_Al) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nQty > 0) ? Global.MillionFormat(nQty) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);

                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }
        }
        private void Print_DeliverySchedule()
        {
            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (_nUsagesHeight > 720)
            {
                _nUsagesHeight = 0;
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                this.PrintHeader();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            if (_oFebricDeliverySchedules.Count > 0)
            {
                #region Total
                PdfPTable oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] { 90f, 40f, 40f, 40f, 85f, 90f });

                _oPdfPCell = new PdfPCell(new Phrase("Fabric Delivery Detail ", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 6;
                _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("Delivery ", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);
                if (_oFSC.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFSC.OrderType == (int)EnumFabricRequestType.Sample || _oFSC.OrderType == (int)EnumFabricRequestType.Bulk || _oFSC.OrderType == (int)EnumFabricRequestType.StockSale || _oFSC.OrderType == (int)EnumFabricRequestType.Buffer)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Qty(Y)", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Set(s)", _oFontStyleBold));
                }
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Note", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyleBold));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (FabricDeliverySchedule oItem in _oFebricDeliverySchedules)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DeliveryDateSt, _oFontStyle));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(oItem.Qty, 0)), _oFontStyle));
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    if (_oFSC.Qty > 0)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round((oItem.Qty * 100 / _oFSC.Qty), 0)), _oFontStyle));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    if (!oItem.IsFoc)
                    {
                        _nTotalDSQty = _nTotalDSQty + oItem.Qty;
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyleBold));
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                if (_oFSC.OrderType == (int)EnumFabricRequestType.SampleFOC || _oFSC.OrderType == (int)EnumFabricRequestType.Sample || _oFSC.OrderType == (int)EnumFabricRequestType.Bulk || _oFSC.OrderType == (int)EnumFabricRequestType.StockSale)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(Math.Round(_nTotalDSQty, 0)) + " Y", _oFontStyleBold));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                }


                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion
            }
        }
        private void Print_Instruction()
        {
          
            string sNotes = "";

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            if (_nUsagesHeight > 620)
            {
                _nUsagesHeight = 0;
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                this.PrintHeader();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            #region
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight =10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] {40f, 450f });
            _oPhrase = new Phrase();
            if (!String.IsNullOrEmpty(_oFSC.EndUse))
            {
                _oPhrase.Add(new Chunk("End Use:", _oFontStyle));
                _oPhrase.Add(new Chunk(_oFSC.EndUse, _oFontStyleBold));
            }
            if (!String.IsNullOrEmpty(_oFSC.LightSourceName))
            {
                _oPhrase.Add(new Chunk(" Light Source:", _oFontStyle));
                if (!String.IsNullOrEmpty(_oFSC.LightSourceName) && !String.IsNullOrEmpty(_oFSC.LightSourceNameTwo))
                {
                    _oPhrase.Add(new Chunk(" Primary:", _oFontStyle));
                }
                _oPhrase.Add(new Chunk(_oFSC.LightSourceName, _oFontStyleBold));
                if (!String.IsNullOrEmpty(_oFSC.LightSourceNameTwo))
                {
                    _oPhrase.Add(new Chunk(" Secondary:", _oFontStyle));
                    _oPhrase.Add(new Chunk(_oFSC.LightSourceNameTwo, _oFontStyleBold));
                }
            }
            if (!String.IsNullOrEmpty(_oFSC.GarmentWash))
            {
                _oPhrase.Add(new Chunk(" Garments Wash Type:", _oFontStyle));
                _oPhrase.Add(new Chunk(_oFSC.GarmentWash, _oFontStyleBold));
            }
            _oPdfPCell = new PdfPCell(_oPhrase);
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

         
            _oPhrase = new Phrase();
            if (!String.IsNullOrEmpty(_oFSC.QualityParameters))
            {
                _oPhrase.Add(new Chunk("Quality Parameters:", _oFontStyle));
                _oPhrase.Add(new Chunk(_oFSC.QualityParameters, _oFontStyleBold));
            }

            if (!String.IsNullOrEmpty(_oFSC.QtyTolarance))
            {
                _oPhrase.Add(new Chunk("  Quantity Tolerance:", _oFontStyle));
                _oPhrase.Add(new Chunk(_oFSC.QtyTolarance, _oFontStyleBold));
            }
            _oPdfPCell = new PdfPCell(_oPhrase);
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Special Instruction(s)", _oFontStyle));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            if (_oFabricSalesContractNotes.Count > 0 &&  _oFEOS.IsOutSide==false)
            {
                _oPdfPCell = new PdfPCell(new Phrase("PO", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (FabricSalesContractNote oItem in _oFabricSalesContractNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.Note + "\n";
                }
                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (_oFabricSpecificationNotes.Count > 0)
            {
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                sNotes = "";
                _oPdfPCell = new PdfPCell(new Phrase("RnD", _oFontStyle));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                _nCount = 0;
                foreach (FabricSpecificationNote oItem in _oFabricSpecificationNotes)
                {
                    _nCount++;
                    sNotes = sNotes + _nCount.ToString() + ". " + oItem.Note + "\n";
                }


                _oPdfPCell = new PdfPCell(new Phrase(sNotes, _oFontStyleBold));
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            //if (_oFSC.CurrentStatus == EnumFabricPOStatus.Cancel)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Cancel Reason:", _oFontStyle));
            //    //_oPdfPCell.Border = 0;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(_oPdfPCell);
            //    _nCount = 0;
            //    _oFabricSalesContract.Note = _oFabricSCHistorys.Where(m => m.FabricSCID == _oFabricSalesContract.FabricSalesContractID && m.FSCStatus == EnumFabricPOStatus.Cancel).First().Note;

            //    _oPdfPCell = new PdfPCell(new Phrase(_oFabricSalesContract.Note, _oFontStyle));
            //    //_oPdfPCell.Border = 0;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //}


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        private void PrintFooter_Default()
        {

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (_nUsagesHeight <780)
            {
                _nUsagesHeight = 780 - _nUsagesHeight;
            }
            else
            { _nUsagesHeight = 10; }
           
            if (_nUsagesHeight > 2)
            {
                #region Blank Row
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, _nUsagesHeight, _oFontStyle);
             
                #endregion
            }
        

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 150f, 150f, 150f });
        #endregion
        #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "_____________", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "_____________", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "_____________", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Prepared By", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Checked By", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Approved By", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }
        #endregion

        #region JobOrder
        public byte[] PrepareReportJobOrder(FabricExecutionOrderSpecification oFEOS, FabricSCReport oFSR, FabricSalesContract oFSC, Fabric oFabric, Company oCompany, List<FabricDeliverySchedule> oFebricDeliverySchedules, List<FabricSalesContractNote> oFabricSalesContractNotes, List<FabricSpecificationNote> oFabricSpecificationNotes, BusinessUnit oBusinessUnit, List<FabricDyeingRecipe> oFabricDyeingRecipes)
        {
            _oFEOS = oFEOS;
            _oFSR = oFSR;
            _oFSC = oFSC;
            _oBusinessUnit = oBusinessUnit;
            _oFebricDeliverySchedules = oFebricDeliverySchedules;
            _oFabricSalesContractNotes = oFabricSalesContractNotes;
            _oFabricSpecificationNotes = oFabricSpecificationNotes;
            _oFabricDyeingRecipes = oFabricDyeingRecipes;
            _oCompany = oCompany;
            _oFabric = oFabric;
            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = new List<string> { "Prepared By", "Checked By", "Approve By" };
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 545f });
            #endregion
            this.PrintHeader_Two();
            this.ReporttHeader();
            this.PrintBody_JobOrder();
            this.PrintFabricSalesContractNote();
            this.Print_Instruction();
            this.Print_DeliverySchedule();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintHeader_Two()
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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print

            _oPdfPCell = new PdfPCell(new Phrase("JOB ORDER", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
          
            #endregion
        }
        private void PrintBody_JobOrder()
        {
            string sExcNo = "";
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyle, true);

            PdfPTable oPdfPTable;
            var fontBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            var fontNormal = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            #region Make New Table
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.SetWidths(new float[] { 72f, 140f,5f, 70f, 80f,5f, 70f,65f });
            #endregion
            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Job Order Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" + "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + Global.MillionFormat(_oFSR.Qty) + " y", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grey Rec'd Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.SCNoFull, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Process Qty ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + Global.MillionFormat(_oFSR.Qty) + " y", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivery Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.FabricReceiveDateStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Concern Person", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.MKTPName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #endregion

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyle, true);
            #region Make New Table
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 100f, 150f, 5f, 100f, 150f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grey Construction", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " "+ _oFEOS.WarpCount + "X" + _oFEOS.WeftCount + "/" +_oFEOS.FinishEnd+"X"+_oFEOS.FinishPick, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finishing Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Composition", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Width", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.FabricWidth+ "\xB''", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color Fastness", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.WeftColor, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weight", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " " + _oFSR.Weight, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shrinkage", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontNormal);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.Shrinkage, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyle, true);



            #endregion

            #region Color Detail
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] {80f, 100f, 60f, 100f, 150f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Mkt Ref No", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade/Color", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Process Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color App No.", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Remarks", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.FabricNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable,Global.MillionFormatActualDigit(Math.Round(_oFEOS.GreigeDemand)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFSR.Note, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, fontBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
          
        }
        public void PrintFabricSalesContractNote()
        {
            var sString = " ";
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 5f, _oFontStyle, true);

            #region Print Header
            _oPdfPCell = new PdfPCell(new Phrase("Process req", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.GRAY;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            foreach (FabricDyeingRecipe oItem in _oFabricDyeingRecipes)
            {
                sString = sString + oItem.Name + " +";
            }
            #region FabricDyeingRecipes Print Body
            sString = sString.Substring(0, sString.Length - 1);
            _oPdfPCell = new PdfPCell(new Phrase(sString, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion
        private void PrintWaterMark(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            string _sWaterMark = "";
            if (_oFEOS.ApproveBy == 0)
            {
                _sWaterMark = "Unauthorised";
            }
           
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            ESimSolWM_Footer.WMFontSize = 80;
            ESimSolWM_Footer.WMRotation = 45;
            ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
            PageEventHandler.WaterMark = _sWaterMark; //Footer print with page event handler
            //PageEventHandler.FooterNote = _oBusinessUnit.ShortName + " Concern Person: " + _oDyeingOrder.MKTPName;
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 500f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
    }
}
