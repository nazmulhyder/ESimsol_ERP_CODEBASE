using ICS.Core.Utility;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class ExportToExcel
    {
        #region Variables
        static int _nRowIndex = 2;
        static int _nColumnIndex = 2;
        static ExcelRange _oCell;
        static ExcelFill _oFill;
        static OfficeOpenXml.Style.Border _oBorder;
        #endregion

        public static string WorksheetName { get; set; }

        public static byte[] ConvertToExcel(PdfPTable oPdfPTable)
        {
            _nRowIndex = 2;
            _nColumnIndex = 2;
            _oCell = null;
            _oFill = null;
            _oBorder = null;           

            ExcelPackage oExcelPackage = new ExcelPackage();
            oExcelPackage.Workbook.Properties.Author = "ESimSol";
            oExcelPackage.Workbook.Properties.Title = "Export from ESimSol";
            ExcelWorksheet oWorkSheet = oExcelPackage.Workbook.Worksheets.Add(ExportToExcel.WorksheetName);

            int nColumnIndex = 2, nFirstColumnIndex = 2;
            List<float> oColumnWidths = ExportToExcel.RequiredColumns(oPdfPTable);
            foreach (float nWidth in oColumnWidths)
            {
                oWorkSheet.Column(nColumnIndex).Width = (nWidth / 4.5);
                nColumnIndex++;
            }

            int nRowSpan = 0; bool bIncreaseRowIndex = true;
            foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
            {
                bIncreaseRowIndex = true;
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                if (oPdfPCells != null)
                {
                    nColumnIndex = 2;
                    nFirstColumnIndex = nColumnIndex;
                    foreach (PdfPCell oPdfPCell in oPdfPCells)
                    {
                        if (oPdfPCell != null)
                        {
                            if (oPdfPCell.Table != null)
                            {
                                ExportToExcel.SubTableFitToExcel(ref oWorkSheet, oPdfPCell.Table, ref nFirstColumnIndex, oPdfPCell.Width, oColumnWidths);
                                bIncreaseRowIndex = false;
                            }
                            else
                            {
                                string sContent = ((oPdfPCell.Phrase != null && oPdfPCell.Phrase.Count > 0) ? oPdfPCell.Phrase[0].ToString() : "");
                                nRowSpan = oPdfPCell.Rowspan; nRowSpan = (nRowSpan <= 1) ? 0 : nRowSpan - 1;
                                int nColSpan = ExportToExcel.GetRequirdColSpan(oPdfPCell.Width, oColumnWidths, nFirstColumnIndex);
                                nColumnIndex = (nColSpan > 1) ? (nFirstColumnIndex + nColSpan-1) : nFirstColumnIndex;
                                _oCell = oWorkSheet.Cells[_nRowIndex, nFirstColumnIndex, (_nRowIndex + nRowSpan), nColumnIndex];

                                if (nRowSpan > 0)
                                    _oCell.Merge = true;
                                else if (nColumnIndex != nFirstColumnIndex)
                                {
                                    _oCell.Merge = true;
                                }
                                //_oCell.Style.Font.Size = 20; 
                                //_oCell.Style.Font.Bold = true;
                                //_oCell.Style.WrapText = true; 
                                //oWorkSheet.Row(_nRowIndex).Height = oPdfPCell.Height;

                                _oCell.Value = sContent;
                                _oCell.Style.HorizontalAlignment = ExportToExcel.GetHorizontalAlignment(oPdfPCell.HorizontalAlignment);
                                _oCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                _oFill = _oCell.Style.Fill;
                                _oFill.PatternType = ExcelFillStyle.Solid;
                                _oFill.BackgroundColor.SetColor(ExportToExcel.GetColorCode(oPdfPCell.BackgroundColor));

                                _oBorder = _oCell.Style.Border;
                                _oBorder.Bottom.Style = _oBorder.Top.Style = _oBorder.Left.Style = _oBorder.Right.Style = ExportToExcel.GetBorder(oPdfPCell.Border);
                            }
                            nFirstColumnIndex = nColumnIndex + 1;
                        }
                    }
                }
                if (bIncreaseRowIndex)
                {
                    _nRowIndex++;
                }
            }
            return oExcelPackage.GetAsByteArray();
        }

        private static void SubTableFitToExcel(ref ExcelWorksheet oWorkSheet, PdfPTable oPdfPTable, ref int nFirstColumnIndex, float nParentCellWidth, List<float> oColumnWidths)
        {
            float nTotalTableWidth = 0f, nCellWidth = 0f;
            nTotalTableWidth = ExportToExcel.GetTableTotalWidth(oPdfPTable);

            int nColumnIndex = 0, nSubTableFirstColumnIndex = 0;
            nSubTableFirstColumnIndex = nFirstColumnIndex;

            int nRowSpan = 0; bool bIncreaseRowIndex = true;
            foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
            {
                bIncreaseRowIndex = true;
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                if (oPdfPCells != null)
                {
                    nColumnIndex = nSubTableFirstColumnIndex;
                    nFirstColumnIndex = nColumnIndex;
                    foreach (PdfPCell oPdfPCell in oPdfPCells)
                    {
                        nRowSpan = 0;
                        if (oPdfPCell != null)
                        {
                            nCellWidth = ((oPdfPCell.Width / nTotalTableWidth) * nParentCellWidth);
                            if (oPdfPCell.Table != null)
                            {
                                ExportToExcel.SubTableFitToExcel(ref oWorkSheet, oPdfPCell.Table, ref nFirstColumnIndex, nCellWidth, oColumnWidths);
                                bIncreaseRowIndex = false;
                            }
                            else
                            {
                                string sContent = ((oPdfPCell.Phrase != null && oPdfPCell.Phrase.Count > 0) ? oPdfPCell.Phrase[0].ToString() : "");
                                nRowSpan = oPdfPCell.Rowspan; nRowSpan = (nRowSpan <= 1) ? 0 : (nRowSpan - 1);
                                int nColSpan = ExportToExcel.GetRequirdColSpan(nCellWidth, oColumnWidths, nFirstColumnIndex);
                                nColumnIndex = (nColSpan > 1) ? (nFirstColumnIndex + nColSpan-1) : nFirstColumnIndex;
                                _oCell = oWorkSheet.Cells[_nRowIndex, nFirstColumnIndex, (_nRowIndex + nRowSpan), nColumnIndex];

                                if (nRowSpan > 0)
                                    _oCell.Merge = true;
                                else if (nColumnIndex != nFirstColumnIndex) 
                                {
                                    try
                                    {
                                        _oCell.Merge = true;
                                    }
                                    catch (Exception we) 
                                    {
                                        //_oCell = oWorkSheet.Cells[_nRowIndex, nFirstColumnIndex+1, _nRowIndex, nColumnIndex];
                                        //_oCell.Merge = true;
                                    }
                                }
                                //_oCell.Style.Font.Size = 20; 
                                //_oCell.Style.Font.Bold = true;
                                //_oCell.Style.WrapText = true; 
                                //oWorkSheet.Row(_nRowIndex).Height = oPdfPCell.Height;
                                                                
                                _oCell.Value = sContent;
                                _oCell.Style.HorizontalAlignment = ExportToExcel.GetHorizontalAlignment(oPdfPCell.HorizontalAlignment);
                                _oCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                _oFill = _oCell.Style.Fill;
                                _oFill.PatternType = ExcelFillStyle.Solid;
                                _oFill.BackgroundColor.SetColor(ExportToExcel.GetColorCode(oPdfPCell.BackgroundColor));

                                _oBorder = _oCell.Style.Border;
                                _oBorder.Bottom.Style = _oBorder.Top.Style = _oBorder.Left.Style = _oBorder.Right.Style = ExportToExcel.GetBorder(oPdfPCell.Border);
                            }
                            nFirstColumnIndex = nColumnIndex + 1;
                        }
                    }
                }
                if (bIncreaseRowIndex)
                {
                    _nRowIndex++;
                }
            }
        }

        #region Supporting Functions
        private static float GetTableTotalWidth(PdfPTable oPdfPTable)
        {
            MemoryStream oMemoryStream = new MemoryStream();
            Document oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            PdfWriter.GetInstance(oDocument, oMemoryStream);
            oDocument.Open();
            oDocument.Add(oPdfPTable);
            oDocument.Close();

            float nTotalWidth = 0;           
            foreach (float nWidth in oPdfPTable.AbsoluteWidths)
            {
                nTotalWidth = nTotalWidth + nWidth;
            }
            nTotalWidth = nTotalWidth <= 0 ? 1f : nTotalWidth;
            return nTotalWidth;
        }
        private static int GetRequirdColSpan(float nCellWidth, List<float> aColumnWidths, int nColumnIndex)
        {
            int nColSpan = 0;
            float nUptoPreviousWidth = 0f, nUptoNextWidth = 0f;
            for (int i = (nColumnIndex - 2); i < aColumnWidths.Count; i++)
            {
                nColSpan = nColSpan + 1;
                nUptoPreviousWidth = nUptoPreviousWidth + aColumnWidths[i];
                if (nUptoPreviousWidth > nCellWidth)
                {
                    break;
                }
                else
                {
                    nUptoNextWidth = nUptoPreviousWidth + ((i + 1) < aColumnWidths.Count ? aColumnWidths[i + 1] : 0);
                    if (nUptoNextWidth > nCellWidth)
                    {
                        break;
                    }
                }
            }

            if ((nUptoNextWidth - nCellWidth) < (nUptoPreviousWidth - nCellWidth))
            {
                nColSpan = nColSpan + 1;
            }
            return nColSpan;
        }
        private static bool IsSubTableExists(PdfPTable oPdfPTable)
        {            
            foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
            {
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                foreach (PdfPCell oPdfPCell in oPdfPCells)
                {
                    if (oPdfPCell != null)
                    {
                        if (oPdfPCell.Table != null)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private static bool IsSubTableExists(PdfPRow oPdfPRow)
        {
            PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
            foreach (PdfPCell oPdfPCell in oPdfPCells)
            {
                if (oPdfPCell != null)
                {
                    if (oPdfPCell.Table != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static List<float> RequiredColumns2(PdfPTable oPdfPTable)
        {
            List<float> oColumnWidths = new List<float>();
            if (ExportToExcel.IsSubTableExists(oPdfPTable))
            {
                List<object> oColumnWidthObjects = new List<object>();
                List<float> oRowWiseColumnWidths = new List<float>();

                #region Find Out Row Wise Column Widths
                foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
                {
                    if (!IsSubTableExists(oPdfPRow))
                    {
                        oRowWiseColumnWidths = new List<float>();
                        PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                        foreach (PdfPCell oPdfPCell in oPdfPCells)
                        {
                            if (oPdfPCell != null)
                            {
                                oRowWiseColumnWidths.Add(oPdfPCell.Width);
                            }
                        }
                        object oColumnWidthObject = (object)oRowWiseColumnWidths;
                        oColumnWidthObjects.Add(oColumnWidthObject);
                    }
                    else
                    {
                        oRowWiseColumnWidths = new List<float>();
                        PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                        foreach (PdfPCell oPdfPCell in oPdfPCells)
                        {
                            if (oPdfPCell != null)
                            {
                                if (oPdfPCell.Table == null)
                                {
                                    oRowWiseColumnWidths.Add(oPdfPCell.Width);
                                }
                                else
                                {
                                    ExportToExcel.CalculateColumnWidth2(oPdfPCell.Table, oPdfPCell.Width, ref oRowWiseColumnWidths);
                                }
                            }
                        }
                        object oColumnWidthObject = (object)oRowWiseColumnWidths;
                        oColumnWidthObjects.Add(oColumnWidthObject);
                    }
                }
                #endregion
            }
            else
            {
                #region Without Subtable
                oColumnWidths = new List<float>();
                PdfPRow oPdfPRow = oPdfPTable.GetRow(0);
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                foreach (PdfPCell oPdfPCell in oPdfPCells)
                {
                    if (oPdfPCell != null)
                    {
                        oColumnWidths.Add(oPdfPCell.Width);
                    }
                    else
                    {
                        oColumnWidths.Add(10f);
                    }
                }
                #endregion
            }
            return oColumnWidths;
        }
        private static void CalculateColumnWidth2(PdfPTable oPdfPTable, float nParentCellWidth, ref List<float> oRefColumnWidths)
        {
            float nTotalTableWidth = 0f, nCellWidth = 0f;
            nTotalTableWidth = ExportToExcel.GetTableTotalWidth(oPdfPTable);

            List<float> oColumnWidths = new List<float>();
            List<float> oTempColumnWidths = new List<float>();
            foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
            {
                oTempColumnWidths = new List<float>();
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                foreach (PdfPCell oPdfPCell in oPdfPCells)
                {
                    if (oPdfPCell != null)
                    {
                        nCellWidth = ((oPdfPCell.Width / nTotalTableWidth) * nParentCellWidth);
                        if (oPdfPCell.Table == null)
                        {
                            oTempColumnWidths.Add(nCellWidth);
                        }
                        else
                        {
                            ExportToExcel.CalculateColumnWidth2(oPdfPCell.Table, nCellWidth, ref oTempColumnWidths);
                        }
                    }
                }
                if (oTempColumnWidths.Count > oColumnWidths.Count)
                {
                    oColumnWidths = oTempColumnWidths;
                }
            }
            oRefColumnWidths.AddRange(oColumnWidths);
        }



        private static List<float> RequiredColumns(PdfPTable oPdfPTable)
        {
            List<float> oColumnWidths = new List<float>();
            List<float> oTempColumnWidths = new List<float>();
            foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
            {
                oTempColumnWidths = new List<float>();
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                foreach (PdfPCell oPdfPCell in oPdfPCells)
                {
                    if (oPdfPCell != null)
                    {
                        if (oPdfPCell.Table == null)
                        {
                            oTempColumnWidths.Add(oPdfPCell.Width);
                        }
                        else
                        {
                            ExportToExcel.CalculateColumnWidth(oPdfPCell.Table, oPdfPCell.Width, ref oTempColumnWidths);
                        }
                    }
                }
                if (oTempColumnWidths.Count > oColumnWidths.Count)
                {
                    oColumnWidths = oTempColumnWidths;
                }
            }
            return oColumnWidths;
        }        
        private static void CalculateColumnWidth(PdfPTable oPdfPTable, float nParentCellWidth, ref List<float> oRefColumnWidths)
        {
            float nTotalTableWidth = 0f, nCellWidth = 0f;
            nTotalTableWidth = ExportToExcel.GetTableTotalWidth(oPdfPTable);

            List<float> oColumnWidths = new List<float>();
            List<float> oTempColumnWidths = new List<float>();
            foreach (PdfPRow oPdfPRow in oPdfPTable.Rows)
            {
                oTempColumnWidths = new List<float>();
                PdfPCell[] oPdfPCells = oPdfPRow.GetCells();
                foreach (PdfPCell oPdfPCell in oPdfPCells)
                {
                    if (oPdfPCell != null)
                    {
                        nCellWidth = ((oPdfPCell.Width / nTotalTableWidth) * nParentCellWidth);
                        if (oPdfPCell.Table == null)
                        {
                            oTempColumnWidths.Add(nCellWidth);
                        }
                        else
                        {
                            ExportToExcel.CalculateColumnWidth(oPdfPCell.Table, nCellWidth, ref oTempColumnWidths);
                        }
                    }
                }
                if (oTempColumnWidths.Count > oColumnWidths.Count)
                {
                    oColumnWidths = oTempColumnWidths;
                }
            }
            oRefColumnWidths.AddRange(oColumnWidths);
        }
        private static int GetMaxSubTableRow(PdfPTable oPdfPTable, int nIndex)
        {
            var oRowCells = oPdfPTable.Rows[nIndex].GetCells();
            int nMaxRow = 0;
            foreach (PdfPCell oItem in oRowCells)
            {
                if (oItem != null && oItem.Table != null && oItem.Table.Rows.Count() > nMaxRow)
                {
                    nMaxRow = oItem.Table.Rows.Count();
                }
            }
            return nMaxRow;
        }
        private static void UpdateSheet(ref ExcelWorksheet oSheet, PdfPTable oPdfPTable)
        {
            int nTotalRows = oPdfPTable.Rows.Count;
            if (nTotalRows > 0)
            {
                var oFirstRowCells = oPdfPTable.Rows[0].GetCells();
                int nTotalCellOfARow = oFirstRowCells.Length;

                #region Table
                int nColIndex = _nColumnIndex + 1;
                //int nNextColumnIndex = nColIndex + 1;
                int nTempColumnIndex = 0;
                string sCellValue = "";
                int nCountColumn = oPdfPTable.Rows[0].GetCells().Count();
                bool bFlag = true;
                for (int i = 0; i < nTotalRows; i++)
                {
                    nTempColumnIndex = nColIndex;
                    var oRowCells = oPdfPTable.Rows[i].GetCells();
                    for (int j = 0; j < nTotalCellOfARow; j++)
                    {
                        var oCurrentRowCells = oRowCells[j];
                        if (oCurrentRowCells == null)
                        {
                            bFlag = true;
                            continue;
                        }
                        bFlag = false;
                        int nColumnSpan = oCurrentRowCells.Colspan;
                        if (nColumnSpan == 1)
                        {
                            _oCell = oSheet.Cells[_nRowIndex, nTempColumnIndex];
                            nTempColumnIndex++;
                        }
                        else if (nColumnSpan == nCountColumn) //Check
                        {
                            _oCell = oSheet.Cells[_nRowIndex, _nRowIndex, _nRowIndex, nCountColumn];
                            if (_nRowIndex != nCountColumn)
                            {
                                _oCell.Merge = true;
                            }
                        }
                        else
                        {
                            nColIndex = nColIndex + nColumnSpan;
                            _oCell = oSheet.Cells[_nRowIndex, nColumnSpan, _nRowIndex, nColIndex];
                            if(nColumnSpan != nColIndex)
                            {
                                _oCell.Merge = true;
                            }
                        }

                        sCellValue = ((oCurrentRowCells.Phrase != null && oCurrentRowCells.Phrase.Count > 0) ? oCurrentRowCells.Phrase[0].ToString() : "");
                        sCellValue = sCellValue.Replace(",", ""); //May be no need
                        _oCell.Value = sCellValue;

                        _oCell.Style.HorizontalAlignment = ExportToExcel.GetHorizontalAlignment(oCurrentRowCells.HorizontalAlignment);
                        _oCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        _oCell.Style.Font.Bold = false;

                        _oFill = _oCell.Style.Fill;
                        _oFill.PatternType = ExcelFillStyle.Solid;
                        _oFill.BackgroundColor.SetColor(ExportToExcel.GetColorCode(oCurrentRowCells.BackgroundColor));
                        _oBorder = _oCell.Style.Border;
                        _oBorder.Bottom.Style = _oBorder.Top.Style = _oBorder.Left.Style = _oBorder.Right.Style = ExportToExcel.GetBorder(oCurrentRowCells.Border);

                        if (i == nTotalRows - 1)
                        {
                            bFlag = true;
                        }
                    }
                    _nRowIndex++;
                    //if (!bFlag) {
                    //    _nRowIndex++;
                    //}
                }
                _nColumnIndex = nTempColumnIndex;
                #endregion
            }
        }
        private static ExcelBorderStyle GetBorder(int nBorder)
        {
            if (nBorder == 15) //None
            {
                return ExcelBorderStyle.Thin;
            }
            else if (nBorder == -1) //bold not found in pdfTable
            {
                return ExcelBorderStyle.Medium;
            }
            return ExcelBorderStyle.None;
        }
        private static Color GetColorCode(BaseColor myColor)
        {
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            if (myColor != null)
            {
                string hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2"); //In Hex
                colFromHex = System.Drawing.ColorTranslator.FromHtml("#" + hex);
            }
            return colFromHex;
        }
        private static ExcelHorizontalAlignment GetHorizontalAlignment(int nHorizontalAlignment)
        {
            if (nHorizontalAlignment == 1) //Center
            {
                return ExcelHorizontalAlignment.Center;
            }
            else if (nHorizontalAlignment == 2) //Right
            {
                return ExcelHorizontalAlignment.Right;
            }
            return ExcelHorizontalAlignment.Left;
        }
        #endregion
    }
}
