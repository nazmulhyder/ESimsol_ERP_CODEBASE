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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptRouteSheetProgress
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<RouteSheet> _oRouteSheets = new List<RouteSheet>();
        List<RouteSheetHistory> _oRouteSheetHistorys = new List<RouteSheetHistory>();
        List<RouteSheetHistory> _oRSDyeingProgess = new List<RouteSheetHistory>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        RouteSheetSetup _oRouteSheetSetup = new RouteSheetSetup();
        int _nColCount = 13;
        double nQty = 0;
        #endregion
        public byte[] PrepareReport(List<RouteSheet> oRouteSheets, List<RouteSheetHistory> oRouteSheetHistorys, List<RouteSheetHistory> oRSDyeingProgess, BusinessUnit oBusinessUnit, Company oCompany, int nReportLayout, RouteSheetSetup oRouteSheetSetup)
        {
            _oRouteSheets = oRouteSheets;
            _oRouteSheetHistorys = oRouteSheetHistorys;
            _oRSDyeingProgess = oRSDyeingProgess;
            _oRouteSheetSetup = oRouteSheetSetup;

            //_oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);

            _oPdfPTable = new PdfPTable(1);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {842});
            #endregion

            #region Report Body & Header
            oCompany.Name = _oBusinessUnit.Name;

            if (nReportLayout == 1) 
            {
                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dyeing Progress Report (Dyeing Machine)", 1);
                this.PrintBody(4); _oPdfPTable.CompleteRow();
                _oPdfPTable.HeaderRows = 2;
                ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);
                _oPdfPTable = new PdfPTable(1);
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.SetWidths(new float[] { 842 });

                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dyeing Progress Report (Hydro)", 1);
                this.PrintBody(2); _oPdfPTable.CompleteRow();
                _oPdfPTable.HeaderRows = 2;
                ESimSolPdfHelper.NewPageDeclaration(_oDocument,_oPdfPTable);
                _oPdfPTable = new PdfPTable(1);
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.SetWidths(new float[] { 842 });

                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dyeing Progress Report (Driyer)", 1);
                this.PrintBody(3); _oPdfPTable.CompleteRow();
                _oPdfPTable.HeaderRows = 2;
                ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);
            }
            else  if (nReportLayout == 2)
            {
                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dyeing Progress Report (Hydro)", 1);
                this.PrintBody(nReportLayout); _oPdfPTable.CompleteRow();
                ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);

                _oPdfPTable.HeaderRows = 2;
            }
            else if (nReportLayout == 3)
            {
                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dyeing Progress Report (Driyer)", 1);
                this.PrintBody(nReportLayout); _oPdfPTable.CompleteRow();
                ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);

                _oPdfPTable.HeaderRows = 2;
            }
            else if (nReportLayout == 4)
            {
                ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oCompany, "Dyeing Progress Report (Dyeing Machine)", 1);
                this.PrintBody(nReportLayout); _oPdfPTable.CompleteRow();
                ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);

                _oPdfPTable.HeaderRows = 2;
            }
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public PdfPTable GetDetailsTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(_nColCount);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[]{
                                                20f,
                                                45f,
                                                50f,
                                                110f,
                                                60f,

                                                70f,
                                                60f,
                                                70f,
                                                
                                                56f,
                                                60f,
                                                45f,
                                                35f,
                                                25f
            });
            return oPdfPTable;
        }
      
        #region Report Body Sale Invoice
        private void PrintBody(int eReportLayout)
        {
            #region Layout Wise Header (Declaration)

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            #endregion

            #region Group By Layout Wise
            //var dataGrpList = _oRouteSheets.GroupBy(x => new { QCDateSt = x.QCDateSt, ShiftName = x.ShiftName }, (key, grp) => new
            //{
            //    HeaderName = key.QCDateSt + " [" + key.ShiftName + "]",
            //    Results = grp.OrderBy(x=>x.ProductName).ThenBy(x=> x.ProductID).ToList()
            //});
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = GetDetailsTable();

            #region Column Header
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "SL#", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Batch No", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Order No", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Name", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Machine Name", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Load Time", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Load By", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "UnLoad Time", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "UnLoad By", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Yarn Type", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shade", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Qty("+_oRouteSheetSetup.MUnit+")", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Remarks", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            _oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            _nColCount = 13;
          
            #region Data List Wise Loop

            int nCount = 0;
            foreach (var obj in _oRouteSheets)
            {
                var oRSDProgress = _oRSDyeingProgess.Where(x => x.RouteSheetID == obj.RouteSheetID).FirstOrDefault();
                var oRSHistory = _oRouteSheetHistorys.Where(x => x.RouteSheetID == obj.RouteSheetID).ToList();

                var oMachine_LD = new RouteSheetHistory();
                var oMachine_ULD = new RouteSheetHistory();
                  
                if (eReportLayout == 2)
                {
                    oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInHydro).FirstOrDefault();
                    oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromHydro).FirstOrDefault();
                }
                else if (eReportLayout == 3)
                {
                    oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInDrier).FirstOrDefault();
                    oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnLoadedFromDrier).FirstOrDefault();
                }
                else if (eReportLayout == 4)
                {
                    oMachine_LD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.LoadedInDyeMachine).FirstOrDefault();
                    oMachine_ULD = oRSHistory.Where(x => x.CurrentStatus == EnumRSState.UnloadedFromDyeMachine).FirstOrDefault();
                }

                if (oMachine_LD != null || oMachine_ULD != null)
                {
                    #region DATA
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    oPdfPTable = new PdfPTable(13);
                    oPdfPTable = GetDetailsTable();

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.OrderNoFull, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    if (eReportLayout == 2 && oRSDProgress != null)
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oRSDProgress.MachineName_Hydro, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    else if (eReportLayout == 3 && oRSDProgress != null)
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oRSDProgress.MachineName_Dryer, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    else if (eReportLayout == 4 && oRSDProgress != null)
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.MachineName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    else
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (oMachine_LD == null ? "" : oMachine_LD.EventTimeStr), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (oMachine_LD == null ? "" : oMachine_LD.UserName), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (oMachine_ULD == null ? "" : oMachine_ULD.EventTimeStr), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (oMachine_ULD == null ? "" : oMachine_ULD.UserName), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.Qty, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (oMachine_ULD == null ? "" : oMachine_ULD.Note), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    nQty = nQty + obj.Qty;
                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    #endregion
                }
            }
            oPdfPTable = new PdfPTable(13);
            oPdfPTable = GetDetailsTable();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 11, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, nQty, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            #endregion
        }
        #endregion
    }
}
