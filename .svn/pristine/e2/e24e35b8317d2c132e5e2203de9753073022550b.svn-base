using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{
    public class rptFabricMachines
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricMachine> _oFabricMachines = new List<FabricMachine>();
        List<TextileSubUnit> _oTextileSubUnits = new List<TextileSubUnit>();
        Company _oCompany = new Company();
        int _nProcessType = -1;
        #endregion

        public byte[] PrepareReport(List<FabricMachine> oFabricMachines, List<TextileSubUnit> oTextileSubUnits, Company oCompany, int nProcessType)
        {
            _oFabricMachines = oFabricMachines;
            _oTextileSubUnits = oTextileSubUnits;
            _oCompany = oCompany;
            _nProcessType = nProcessType;
            
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeaderDetail(string sReportHead, string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            string sTitle = "";
            if (_nProcessType == 0) sTitle = "Warping";
            else if (_nProcessType == 1) sTitle = "Sizing";
            else if (_nProcessType == 2) sTitle = "Drawing In";
            else if (_nProcessType == 3) sTitle = "Loom";
            else sTitle = " ";

            this.PrintHeaderDetail(sTitle + " Management", " ", " ");
            this.SetData1stTable();
            this.SetData2ndTable();
            this.SetData3rdTable();
        }
        #endregion

        private void SetData3rdTable()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            var data = _oFabricMachines.GroupBy(x => new { x.Capacity }, (key, grp) => new
            {
                Capacity = key.Capacity,
                Results = grp.ToList().OrderBy(y => y.Capacity)
            });

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 15f, 15f, 15f, 15f, 10f });
            int nTotalRow = 0;
            foreach (var oData in data)
                nTotalRow += oData.Results.Count();
            int sl = 0;
            foreach (var oData in data)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.Capacity, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.Results.Sum(x => x.RPM).ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                if (sl == 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricMachines.Sum(z => z.RPM).ToString("#,##0.00;(#,##0.00)"), nTotalRow, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);

                oPdfPTable.CompleteRow();
                sl++;
            }
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, _oFontStyleBold);
        }

        private void SetData2ndTable()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            var data = _oFabricMachines.GroupBy(x => new { x.ChildMachineTypeID, x.ChildMachineTypeName, x.Capacity }, (key, grp) => new
            {
                ChildMachineTypeID = key.ChildMachineTypeID,
                ChildMachineTypeName = key.ChildMachineTypeName,
                Results = grp.ToList().OrderBy(y => y.ChildMachineTypeID).ThenBy(z => z.Capacity)
            });

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 10f, 20f, 15f, 15f, 15f, 15f, 10f });
            int nTotalRow =0;
            foreach (var oData in data)
                nTotalRow += oData.Results.Count();

            #region header
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub-Category", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Quantity", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub-Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            #endregion
            int sl = 0;
            foreach (var oData in data)
            {
                #region Data
                int nChildMachineTypeID = -999;
                foreach (var oItem in oData.Results)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                    if (nChildMachineTypeID != oItem.ChildMachineTypeID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ChildMachineTypeName, oData.Results.Where(x => x.ChildMachineTypeID == oItem.ChildMachineTypeID).Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Capacity, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RPM.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    if (nChildMachineTypeID != oItem.ChildMachineTypeID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.Results.Where(x => x.ChildMachineTypeID == oItem.ChildMachineTypeID).Sum(z => z.RPM).ToString("#,##0.00;(#,##0.00)"), oData.Results.Where(x => x.ChildMachineTypeID == oItem.ChildMachineTypeID).Count(), 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    if (sl == 0)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricMachines.Sum(z => z.RPM).ToString("#,##0.00;(#,##0.00)"), nTotalRow, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);

                    sl++;
                    nChildMachineTypeID = oItem.ChildMachineTypeID;
                    oPdfPTable.CompleteRow();
                }
                #endregion
                
            }
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 30, _oFontStyleBold);
        }

        private void SetData1stTable()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            var data = _oFabricMachines.GroupBy(x => new { x.TSUID, x.TextileSubUnitName }, (key, grp) => new
            {
                TSUID = key.TSUID,
                TextileSubUnitName = key.TextileSubUnitName,
                Results = grp.ToList().OrderBy(y => y.TSUID).ThenBy(y => y.ParentMachineTypeID).ThenBy(z => z.ChildMachineTypeID).ThenBy(z => z.FabricMachineGroupID).ThenBy(z => z.Capacity)
            });
            
            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 18f, 18f, 18f, 12f, 12f, 12f, 12f });
            foreach (var oData in data)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 18f, 18f, 18f, 12f, 12f, 12f, 12f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.TextileSubUnitName, 0, 7, Element.ALIGN_CENTER, BaseColor.WHITE, true, 15, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                #region header
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Category", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub-Category", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Group Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Quantity", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sub-Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                #endregion

                int nParentMachineTypeID = -999, nChildMachineTypeID = -999, nTSUID = -999, nFabricMachineGroupID = -999;
                foreach (var oItem in oData.Results)
                {
                    #region Data
                    if (nParentMachineTypeID != oItem.ParentMachineTypeID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ParentMachineTypeName, oData.Results.Where(x => x.ParentMachineTypeID == oItem.ParentMachineTypeID).Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    if (nChildMachineTypeID != oItem.ChildMachineTypeID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ChildMachineTypeName, oData.Results.Where(x => x.ParentMachineTypeID == oItem.ParentMachineTypeID && x.ChildMachineTypeID == oItem.ChildMachineTypeID).Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    //if (nFabricMachineGroupID != oItem.FabricMachineGroupID)
                    //{
                    //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FabricMachineGroupName, oData.Results.Where(x => x.ParentMachineTypeID == oItem.ParentMachineTypeID && x.ChildMachineTypeID == oItem.ChildMachineTypeID && x.FabricMachineGroupID == oItem.FabricMachineGroupID).Count(), 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //}
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FabricMachineGroupName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Capacity, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RPM.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    if (nChildMachineTypeID != oItem.ChildMachineTypeID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.Results.Where(x => x.ParentMachineTypeID == oItem.ParentMachineTypeID && x.ChildMachineTypeID == oItem.ChildMachineTypeID).Sum(z => z.RPM).ToString("#,##0.00;(#,##0.00)"), oData.Results.Where(x => x.ParentMachineTypeID == oItem.ParentMachineTypeID && x.ChildMachineTypeID == oItem.ChildMachineTypeID).Count(), 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    if (nTSUID != oData.TSUID)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oData.Results.Sum(x=>x.RPM).ToString("#,##0.00;(#,##0.00)"), oData.Results.Count(), 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    nParentMachineTypeID = oItem.ParentMachineTypeID;
                    nChildMachineTypeID = oItem.ChildMachineTypeID;
                    nFabricMachineGroupID = oItem.FabricMachineGroupID;
                    nTSUID = oData.TSUID;
                    #endregion
                }

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, _oFontStyleBold);
            }

            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, " ", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, _oFontStyleBold);
        }

    }
}
