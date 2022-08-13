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
    public class rptDUPSchedule
    {
        #region Declaration
        int _nColumns;
        int _nTempCol;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUPSchedule> _oDUPSchedules = new List<DUPSchedule>();
        DUPSchedule _oDUPSchedule = new DUPSchedule();
        Company _oCompany = new Company();
        double _nTotalPSQty = 0, _nGrandTotal = 0, _nTotalQtyInHouse = 0, _nTotalQtyOutside = 0;
        string _sMessage = "";
        int _nPageWidth = 0;
        int _npageHeight = 595;
        float nUsagesHeight = 0;
        RouteSheetSetup _oRouteSheetSetup = new RouteSheetSetup();

        public List<DUPSLot> DUPSLots = new List<DUPSLot>();
        public List<LotParent> LotParents = new List<LotParent>();
        public List<FabricLotAssign> FabricLotAssigns = new List<FabricLotAssign>();

        List<Machine> _oDyemachines = new List<Machine>();
        int nSLNo = 0;
        DateTime date = DateTime.Now;
        int _nStatus = 0;
        string[] arrayDateName = new string[] { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };

        //int nstartTimeInHour = 0;
        //int nstartTimeInMinute = 0;
        //int nendTimeInHour = 0;
        //int nendTimeInMinute = 0;

        //string sStartDate = "";
        //string sEndDate = "";
        //string sStartTimeInHour = "";
        //string sStartTimeInMinute = "";
        //string sEndTimeInHour = "";
        //string sEndTimeInMinute = "";
       int _nMaxValue=0;
       float nMinimumHeight = 15f;
        #endregion

        #region Constructor

        public rptDUPSchedule() 
        {
            DUPSLots = new List<DUPSLot>();
            LotParents = new List<LotParent>();
            FabricLotAssigns = new List<FabricLotAssign>();
        }

        #endregion

        public byte[] PrepareReport(List<Machine> oDyemachines, RouteSheetSetup oRouteSheetSetup, DUPSchedule oDUPSchedule, Company oCompany, int nMaxValue)
        {
            _oDUPSchedule = oDUPSchedule;
            _oDUPSchedules = oDUPSchedule.DUPSchedules;
            _oDyemachines=oDyemachines;
            _oCompany = oCompany;
            _nMaxValue = nMaxValue;
            if (_nMaxValue <6)
            {
                _nMaxValue = 5;
            }
            _nMaxValue = _nMaxValue - 1; // for Loop start =0
            if(_nMaxValue<6)
            {
                nMinimumHeight = 80;// Row Hight
            }

            _oRouteSheetSetup = oRouteSheetSetup;

            if (_oRouteSheetSetup.MachinePerDoc <= 0)
                _oRouteSheetSetup.MachinePerDoc = 14;
            if (_oRouteSheetSetup.FontSize <= 0)
                _oRouteSheetSetup.FontSize = 8;

            int len = _oDyemachines.Count;

            if (len > _oRouteSheetSetup.MachinePerDoc)
            {
                if ((len % _oRouteSheetSetup.MachinePerDoc) == 0)
                {
                    _nTempCol = len;
                }
                else
                {
                    _nTempCol = len + (_oRouteSheetSetup.MachinePerDoc - (len % _oRouteSheetSetup.MachinePerDoc));
                    _nTempCol = _oRouteSheetSetup.MachinePerDoc;
                }
                _nTempCol = _nColumns + 1;
            }
            else
            {
                _nTempCol = 15;
            }
            _nColumns = _oRouteSheetSetup.MachinePerDoc+1;
            _oPdfPTable = new PdfPTable(_nColumns);

            _nPageWidth = (100 * (_nColumns - 1)) + 40;

            #region Page Setup
           // _oDocument = new Document(new iTextSharp.text.Rectangle( _nPageWidth,_npageHeight), 0f, 0f, 0f, 0f);
            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4.Rotate());
            _oDocument.SetMargins(10f,5f, 10f, 20f);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = new List<string> { "Prepared By", "Checked By", "Approved By" };
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.WidthPercentage = 100;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 20f;
            for (int i = 1; i < _nColumns; i++)
            {
                    tablecolumns[i] = 100f;
            }

            _oPdfPTable.SetWidths(tablecolumns); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion

            this.PrintHeader();
            this.PrintBody();
         //   this.PrintFooter_B();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            int nflag = 0;
            #region CompanyHeader

            if (_oCompany.CompanyLogo != null)
            {
                nflag = 1;
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(55f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.FixedHeight = 25f;
                _oPdfPCell.Border = 0;
                _oPdfPCell.PaddingRight = 20f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            if (nflag == 1)
            {
                _oPdfPCell.Colspan =_nColumns- 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else
            {
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion


            #region DateRange

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Schedule of: " + _oDUPSchedule.OrderInfo, (_oDUPSchedule.Status == EnumProductionScheduleStatus.Urgent) ? _oFontStyleBold : _oFontStyle));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);

            _nTotalQtyInHouse = _oDUPSchedule.DUPScheduleDetails.Where(x => x.IsInHouse == true).Sum(x => x.Qty);
            _nTotalQtyOutside = _oDUPSchedule.DUPScheduleDetails.Where(x => x.IsInHouse == false).Sum(x => x.Qty);
            _nTotalPSQty = _oDUPSchedule.DUPScheduleDetails.Sum(x => x.Qty);
            string sQuantity = "In House Qty: " + Global.MillionFormat(_nTotalQtyInHouse) + " "+_oRouteSheetSetup.MUnit+" Out Side Qty: " + Global.MillionFormat(_nTotalQtyOutside) +
                                " "+_oRouteSheetSetup.MUnit+ " Total Qty: " + Global.MillionFormat(_nTotalPSQty) ;

            _oPdfPCell = new PdfPCell(new Phrase(sQuantity + "     " + DateTime.Now.ToString("dd MMM yyy HH:mm"), _oFontStyle));
            _oPdfPCell.Colspan = _nColumns-3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;

            if (_oDyemachines.Count <= _oRouteSheetSetup.MachinePerDoc)
            {
                TableHeader(_oDyemachines);
                //TableContent(_oDUPSchedule.MaxValue,_oDyemachines);
            }
            else
            {
                List<Machine> oDyemachines = new List<Machine>();
                List<Machine> oDyemachines_PS = new List<Machine>();
                List<DUPSchedule> oPSS = new List<DUPSchedule>();
                oPSS = _oDUPSchedule.DUPSchedules;
                int nCount = 0;
                int nMachineCount = 0;
                foreach (Machine oDyemachine in _oDyemachines)
                {
                    ++nCount;
                    ++nMachineCount;

                    oDyemachines.Add(oDyemachine);
                    if (oDyemachines.Count == _oRouteSheetSetup.MachinePerDoc)
                    {
                        TableHeader(oDyemachines);
                        //TableContent(RetrunMaxValue(oDyemachines, oPSS), oDyemachines);
                        nCount = 0;
                        oDyemachines = new List<Machine>();

                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        this.PrintHeader();
                    }
                    else if (nMachineCount == _oDyemachines.Count)
                    {
                        TableHeader(oDyemachines);
                        //TableContent(RetrunMaxValue(oDyemachines, oPSS), oDyemachines);
                        //nCount = 0;
                        //oDyemachines = new List<CapitalResource>();
                    }

                   
                }
              
            }
        }
        private void AddOrderDetail(Machine oMachine, string sValue)
        {
               foreach(DUPSchedule oDUPSchedule in _oDUPSchedules)
               {
                   if(oDUPSchedule.DBUserID!=5 && oDUPSchedule.MachineID==oMachine.MachineID)
                   {
                       oMachine.Name = oDUPSchedule.OrderInfo;
                       oMachine.IsBold = false;
                       if (!string.IsNullOrEmpty(oMachine.Name))
                       {
                           oMachine.IsBold = (oDUPSchedule.Status == EnumProductionScheduleStatus.Urgent) ? true : false;
                       }
                       oDUPSchedule.DBUserID = 5;
                       break;
                   }
               }
        }
        private Phrase AddOrderDetail_Phase(Machine oMachine, string sValue)
        {
            Phrase oPhase_Order = new Phrase(); 
            foreach (DUPSchedule oDUPSchedule in _oDUPSchedules)
            {
                if (oDUPSchedule.DBUserID != 5 && oDUPSchedule.MachineID == oMachine.MachineID)
                {
                    _nStatus = oDUPSchedule.StatusInt;
                    oPhase_Order = GetOrderInfo(oDUPSchedule);  //oMachine.Name = oDUPSchedule.OrderInfo;
                    oMachine.IsBold = false;
                    if (!string.IsNullOrEmpty(oMachine.Name))
                    {
                        oMachine.IsBold = (oDUPSchedule.Status == EnumProductionScheduleStatus.Urgent) ? true : false;
                    }
                    oDUPSchedule.DBUserID = 5;
                    break;
                }
            }
            return oPhase_Order;
        }

        private Phrase GetOrderInfo(DUPSchedule oDUPSchedule)
        {
            int nCount = 0;
            string sTemp = "";
            Phrase oPhrase = new Phrase();
            Chunk oChunk = new Chunk();
            iTextSharp.text.Font oFont_Bold= new iTextSharp.text.Font();
            oFont_Bold= FontFactory.GetFont("Tahoma", (float)_oRouteSheetSetup.FontSize, 1); 

            oDUPSchedule.DUPScheduleDetails = new List<DUPScheduleDetail>();
            oDUPSchedule.DUPScheduleDetails = _oDUPSchedule.DUPScheduleDetails.Where(x => x.DUPScheduleID == oDUPSchedule.DUPScheduleID).ToList();
            string sDateRange = oDUPSchedule.StartTime.ToString("dd MMM yy") + "(" + oDUPSchedule.StartTime.ToString("HH:mm tt") + "-" + oDUPSchedule.EndTime.ToString("HH:mm tt") + ")";

           

            if (oDUPSchedule.DUPScheduleDetails.Count == 0 || oDUPSchedule.DUPScheduleDetails == null) 
            {
                oChunk = new Chunk(oDUPSchedule.ScheduleNo + " " + sDateRange, _oFontStyle); oPhrase.Add(oChunk);
            }
            else
            {
                oChunk = new Chunk(sDateRange, _oFontStyle); oPhrase.Add(oChunk);
               // oDUPSchedule.LotNo = string.Join("+", DUPSLots.Where(x => x.DUPScheduleID == oDUPSchedule.DUPScheduleID).Select(x => x.LotNo + " (" + Global.MillionFormat(x.Qty) + ")").Distinct().ToList());
                foreach (DUPScheduleDetail oItem in oDUPSchedule.DUPScheduleDetails)
                {
                    if (DUPSLots.FirstOrDefault() != null && DUPSLots.FirstOrDefault().DUPScheduleID > 0 && DUPSLots.Where(b => (b.DUPScheduleID == oItem.DUPScheduleID && b.DUPScheduleDetailID == oItem.DUPScheduleDetailID && b.DODID == oItem.DODID)).Count() > 0)
                    {
                        oItem.BuyerRef = string.Join(",", DUPSLots.Where(x => x.DUPScheduleDetailID == oItem.DUPScheduleDetailID).Select(x => x.LotNo).Distinct().ToList());
                    }
                    if (string.IsNullOrEmpty(oItem.BuyerRef))
                    {
                        if (oItem.IsInHouse)
                        {
                            oDUPSchedule.LotNo = string.Join("+", FabricLotAssigns.Where(x => x.DyeingOrderDetailID == oItem.DODID).Select(x => x.LotNo).Distinct().ToList());
                            oItem.BuyerRef = string.Join("+", FabricLotAssigns.Where(x => x.DyeingOrderDetailID == oItem.DODID).Select(x => x.LotNo).Distinct().ToList());
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(oItem.BuyerRef)) { oDUPSchedule.LotNo = oItem.BuyerRef; }
                            else
                            {
                                oDUPSchedule.LotNo = string.Join("+", LotParents.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).Select(x => x.LotNo).Distinct().ToList());
                                oItem.BuyerRef = string.Join("+", LotParents.Where(x => x.DyeingOrderID == oItem.DyeingOrderID && x.ProductID == oItem.ProductID).Select(x => x.LotNo).Distinct().ToList());
                            }
                        }
                    }
                }
                if (_oRouteSheetSetup.IsShowBuyer)
                { 
                    oChunk = new Chunk( "\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.BuyerName).Distinct().ToList()), oFont_Bold);
                    oPhrase.Add(oChunk);
                }
                else 
                {
                    oChunk = new Chunk("\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.ContractorName).Distinct().ToList()), oFont_Bold);
                    oPhrase.Add(oChunk);
                }

                oChunk = new Chunk("\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.OrderNo).Distinct().ToList()), _oFontStyle);oPhrase.Add(oChunk);
                oChunk = new Chunk("\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.ProductName).Distinct().ToList()), _oFontStyle);oPhrase.Add(oChunk);

                //if (!string.IsNullOrEmpty(oDUPSchedule.LotNo))
                //{
                //    oChunk = new Chunk("\n Lot: " + oDUPSchedule.LotNo, _oFontStyle); oPhrase.Add(oChunk);
                //}
                oChunk = new Chunk("\n Lot:" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.BuyerRef ).Distinct().ToList()), oFont_Bold); oPhrase.Add(oChunk);
                oChunk = new Chunk("\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.ColorName+ (string.IsNullOrEmpty(x.PantonNo)?"": "("+x.PantonNo+")")).Distinct().ToList()), oFont_Bold); oPhrase.Add(oChunk);
                
                sTemp = string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.ColorNo).Distinct().ToList());
                if (!string.IsNullOrEmpty(sTemp))
                {
                    oChunk = new Chunk("\n " + sTemp, _oFontStyle); oPhrase.Add(oChunk);
                }

                sTemp = string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.ApproveLotNo).Distinct().ToList());
                if (!string.IsNullOrEmpty(sTemp))
                {
                    oChunk = new Chunk( "\n (" + sTemp + ")", _oFontStyle); oPhrase.Add(oChunk);
                }

                oChunk = new Chunk( "\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => Global.MillionFormat(x.Qty) + " " + _oRouteSheetSetup.MUnit + ((x.BagCount <= 0) ? "" : " (" + x.BagCount.ToString() + " " + x.HankorConeST + ")")).ToList()), oFont_Bold); oPhrase.Add(oChunk);
                //oDUPSchedule.OrderInfo = oDUPSchedule.OrderInfo +
                if (!string.IsNullOrEmpty(_oRouteSheetSetup.BatchCode))
                {
                    oChunk = new Chunk( "\n" + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => _oRouteSheetSetup.BatchCode + "-" + x.PSBatchNo).Distinct().ToList()), _oFontStyle); oPhrase.Add(oChunk);
                }
                //oRouteSheetSetup.BatchCode + "-" + oItem2.PSBatchNo
                if (!String.IsNullOrEmpty(oDUPSchedule.Note))
                {
                    oChunk = new Chunk("\n (" + oDUPSchedule.Note + ")", _oFontStyle); oPhrase.Add(oChunk);
                }
                if (!String.IsNullOrEmpty(oDUPSchedule.DUPScheduleDetails[0].RouteSheetNo))
                {
                    oChunk = new Chunk("\nBatch No: " + string.Join("+", oDUPSchedule.DUPScheduleDetails.Select(x => x.RouteSheetNo).Distinct().ToList()), _oFontStyle); oPhrase.Add(oChunk);
                }
            }
            return oPhrase;
        }

        private void TableHeader(List<Machine> oDyemachines)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", (float)_oRouteSheetSetup.FontSize, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", (float)_oRouteSheetSetup.FontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (oDyemachines.Count == _oRouteSheetSetup.MachinePerDoc)
            {
                foreach (Machine oDyemachine in oDyemachines)
                {
                    oDyemachine.Name = oDyemachine.Name.Trim();
                    oDyemachine.Code = oDyemachine.Code.Trim();
                    oDyemachine.Capacity2 = oDyemachine.Capacity2.Trim();
                    _oPdfPCell = new PdfPCell(new Phrase( oDyemachine.Code + "(" + oDyemachine.Name + ")\n(" + oDyemachine.Capacity2 + ")", _oFontStyleBold));
                    _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();

                //_nMaxValue Max Order In One Machain
                for (int i = 0; i <= _nMaxValue; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase((i+1).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _nStatus = 0;
                    foreach (Machine oDyemachine in oDyemachines)
                    {
                        oDyemachine.Name = "";
                        //AddOrderDetail(oDyemachine, "");
                        //foreach (DUPSchedule oDUPSchedule in _oDUPSchedules)
                        //{
                        //    if (oDUPSchedule.DBUserID != 5 && oDUPSchedule.MachineID == oDyemachine.CRID)
                        //    {
                        //_oRouteSheetSetup.PrintNo = EnumExcellColumn.A;
                        _nStatus = 0;
                        Phrase oPhase_Order = new Phrase(); oPhase_Order = AddOrderDetail_Phase(oDyemachine, "");
                        _oPdfPCell = new PdfPCell(oPhase_Order);
                        _oPdfPCell.MinimumHeight = nMinimumHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; //_oPdfPCell.BackgroundColor = (oDyemachine.IsBold && oDyemachine.Name != "") ? BaseColor.GRAY : BaseColor.WHITE; 
                        _oPdfPCell.BackgroundColor = (_oRouteSheetSetup.PrintNo == EnumExcellColumn.B) ? (new BaseColor(System.Drawing.ColorTranslator.FromHtml(GetColoCode(_nStatus, oPhase_Order.Count)))) : ((oDyemachine.IsBold && oDyemachine.Name != "") ? BaseColor.GRAY : BaseColor.WHITE);
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPTable.CompleteRow();
                }
            }

            else if (oDyemachines.Count < _oRouteSheetSetup.MachinePerDoc)
            {
                foreach (Machine oDyemachine in oDyemachines)
                {
                    oDyemachine.Name = oDyemachine.Name.Trim();
                    oDyemachine.Code = oDyemachine.Code.Trim();
                    oDyemachine.Capacity2 = oDyemachine.Capacity2.Trim();
                    _oPdfPCell = new PdfPCell(new Phrase(oDyemachine.Code + "(" + oDyemachine.Name + ")\n(" + oDyemachine.Capacity2 + ")", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                }
                for (int i = (_oDyemachines.Count + 1); i < 15; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();
                for (int i = 0; i <= _nMaxValue; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase((i+1).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _nStatus = 0;
                    foreach (Machine oDyemachine in oDyemachines)
                    {
                        oDyemachine.Name = "";
                        //AddOrderDetail(oDyemachine, "");
                        //foreach (DUPSchedule oDUPSchedule in _oDUPSchedules)
                        //{
                        //    if (oDUPSchedule.DBUserID != 5 && oDUPSchedule.MachineID == oDyemachine.CRID)
                        //    {
                        _nStatus = 0;
                        Phrase oDyemachine_Phase = new Phrase();  
                        //_oPdfPCell = new PdfPCell(new Phrase(oDyemachine.Name, _oFontStyle));
                        Phrase oPhase_Order = new Phrase(); oPhase_Order = AddOrderDetail_Phase(oDyemachine, "");
                        _oPdfPCell = new PdfPCell(oPhase_Order);
                        _oPdfPCell.MinimumHeight = nMinimumHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; //_oPdfPCell.BackgroundColor = BaseColor.WHITE; 
                        _oPdfPCell.BackgroundColor = (_oRouteSheetSetup.PrintNo == EnumExcellColumn.B) ? (new BaseColor(System.Drawing.ColorTranslator.FromHtml(GetColoCode(_nStatus, oPhase_Order.Count)))) : (BaseColor.WHITE);
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPTable.CompleteRow();
                }
            }
        }

        private string GetColoCode(int nStatus, int nCount)
        {
            string sColor = "#ffffff";
            if (nCount > 0)
            {
                if (nStatus <= 0)
                {
                    sColor = "#0099FF";
                }
                else if (nStatus == 1)
                {
                    sColor = "#BEA9F0";
                }
                else if (nStatus == 2)
                {
                    sColor = "#ffff4d";
                }
                else if (nStatus == 3)
                {
                    sColor = "#80ff80";
                }
                else if (nStatus == 4)
                {
                    sColor = "#a3a3c2";
                }
                else if (nStatus == 5)//cancel
                {
                    sColor = "#FC0D0D";
                }
                else if (nStatus == 6)
                {
                    sColor = "#1CAB12";
                }
            }
            
            return sColor;
        }
        private void PrintFooter_B()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            #region
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
            if (nUsagesHeight < 300)
            {
                nUsagesHeight = 300 - nUsagesHeight;
            }
            if (nUsagesHeight > 300)
            {
                #region Blank Row


                while (nUsagesHeight < 300)
                {
                    #region Table Initiate
                    PdfPTable oPdfPTableTemp = new PdfPTable(4);
                    oPdfPTableTemp.SetWidths(new float[] { 148f, 148f, 148f, 148f });

                    #endregion

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.FixedHeight = 18f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableTemp.AddCell(_oPdfPCell);


                    oPdfPTableTemp.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                }

                #endregion
            }
            #endregion


            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 197f, 197f, 197f });


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 10f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("__________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("_______________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("_________________", _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyleBold));
            oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPTable.CompleteRow();

        }

        //private void TableContent(int nMaxValue, List<CapitalResource> oDyemachines)
        //{
        //    int nj = 0;
        //    int flag = 0;
        //    int x = 0; ;

        //    if (nMaxValue == 0)
        //    {
        
        //        for (int i = 1; i <= 5; i++)
        //        {

        //            _oPdfPCell = new PdfPCell(new Phrase((++nSLNo).ToString(), _oFontStyle));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);
        //            for (int j = 1; j <= 8; j++)
        //            {

        //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);

        //            }

        //            _oPdfPTable.CompleteRow();
        //        }

        //    }

        //    else
        //    {
        //        int len = nMaxValue;
        //        int nRows = len;
        //        // int nRows = 0;

        //        if (len > 5)
        //        {
        //            if ((len % 5) == 0)
        //            {
        //                nRows = len;
        //            }
        //            else
        //            {
        //                nRows = len + (5 - (len % 5));
        //            }

        //        }
        //        else
        //        {
        //            nRows = 5;
        //        }


        //        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

        //        if (nMaxValue > 0)
        //        {

        //            for (x = 1; x <= nRows; x++)
        //            {
        //                _oPdfPCell = new PdfPCell(new Phrase((++nSLNo).ToString(), _oFontStyle));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);


        //                foreach (CapitalResource oItems in oDyemachines)
        //                {
        //                    flag = 0;

        //                    for (nj = 0; nj < _oDUPSchedule.DUPScheduleList.Count; nj++)
        //                    {

        //                        if (_oDUPSchedule.DUPScheduleList[nj].MachineID == oItems.CRID)
        //                        {

        //                            sStartDate = _oDUPSchedule.DUPScheduleList[nj].StartTime.ToString("dd MMM yyyy");
        //                            sEndDate = _oDUPSchedule.DUPScheduleList[nj].EndTime.ToString("dd MMM yyyy");
        //                            nstartTimeInHour = _oDUPSchedule.DUPScheduleList[nj].StartTime.Hour;
        //                            nstartTimeInMinute = _oDUPSchedule.DUPScheduleList[nj].StartTime.Minute;
        //                            nendTimeInHour = _oDUPSchedule.DUPScheduleList[nj].EndTime.Hour;
        //                            nendTimeInMinute = _oDUPSchedule.DUPScheduleList[nj].EndTime.Minute;

        //                            sStartTimeInHour = nstartTimeInHour.ToString();
        //                            sStartTimeInMinute = nstartTimeInMinute.ToString();
        //                            sEndTimeInHour = nendTimeInHour.ToString();
        //                            sEndTimeInMinute = nendTimeInMinute.ToString();

        //                            if (nstartTimeInHour < 10)
        //                            {
        //                                sStartTimeInHour = "";
        //                                sStartTimeInHour = "0" + nstartTimeInHour;
        //                            }
        //                            if (nstartTimeInMinute < 10)
        //                            {
        //                                sStartTimeInMinute = "";
        //                                sStartTimeInMinute = "0" + nstartTimeInMinute;
        //                            }
        //                            if (nendTimeInHour < 10)
        //                            {
        //                                sEndTimeInHour = "";
        //                                sEndTimeInHour = "0" + nendTimeInHour;
        //                            }
        //                            if (nendTimeInMinute < 10)
        //                            {
        //                                sEndTimeInMinute = "";
        //                                sEndTimeInMinute = "0" + nendTimeInMinute;
        //                            }

                                  

        //                            PdfPTable oTempTable = new PdfPTable(1);
        //                            oTempTable.SetWidths(new float[] { 98f });
        //                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

        //                            List<DUPScheduleDetail> oTempPSDs = new List<DUPScheduleDetail>();
        //                            oTempPSDs = _oDUPSchedule.DUPScheduleList[nj].DUPScheduleDetails;

        //                            string sFactory = string.Join(", ", (from p in oTempPSDs where p.FactoryName.Trim() != "" select p.FactoryName.Trim()).Distinct().ToList());
        //                            string sBuyerRef = string.Join(", ", (from p in oTempPSDs where p.BuyerRef.Trim() != "" select p.BuyerRef.Trim()).Distinct().ToList());
        //                            string sOrderNo = string.Join(", ", (from p in oTempPSDs where p.OrderNo.Trim() != "" select p.OrderNo.Trim()).Distinct().ToList());
        //                            string sRSState = string.Join(", ", (from p in oTempPSDs where p.RSStateInString.Trim() != "" select p.RSStateInString.Trim()).Distinct().ToList());
        //                            string sYarnName = string.Join(", ", (from p in oTempPSDs where p.ProductName.Trim() != "" select p.ProductName.Trim()).Distinct().ToList());
        //                            string sColorName = string.Join(", ", (from p in oTempPSDs where p.ColorName.Trim() != "" select p.ColorName.Trim() + "/" + p.PSBatchNo).Distinct().ToList());
        //                            string sBatchCardNo = string.Join(", ", (from p in oTempPSDs where p.BatchCardNo.Trim() != "" select p.BatchCardNo).Distinct().ToList());
        //                            string sRemarks = string.Join(", ", (from p in oTempPSDs where p.Remarks.Trim() != "" select p.Remarks.Trim()).Distinct().ToList());
        //                            string sTotal = Global.MillionFormat(Convert.ToDouble((from p in oTempPSDs select p.ProductionQty).Sum())) + " kg ";
        //                            string sDeliveryDate = (oTempPSDs.Where(o => o.ExpDeliveryDateByFactory != DateTime.MinValue).ToList().Count() > 0) ? oTempPSDs.Where(o => o.ExpDeliveryDateByFactory != DateTime.MinValue).Min(o => o.ExpDeliveryDateByFactory).ToString("dd MMM yyyy") : "";
                                    
        //                            _oPdfPCell = new PdfPCell(new Phrase(sFactory, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase("BYR- " + sBuyerRef, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sOrderNo, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sRSState, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sYarnName, _oFontStyleBold));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sColorName, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sBatchCardNo, _oFontStyleBold));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sTotal, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sRemarks, _oFontStyleBold));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                            _oPdfPCell = new PdfPCell(new Phrase(sDeliveryDate, _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);

        //                            oTempTable.CompleteRow();

        //                            //foreach (DUPScheduleDetail oPSD in oTempPSDs)
        //                            //{
        //                                //if (oPSD.BuyerName == "" || (oPSD.BuyerName.Trim() == oPSD.FactoryName.Trim()))
        //                                //{
        //                                //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.FactoryName.Trim(), _oFontStyle));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase("BYR- " + oPSD.BuyerRef.Trim(), _oFontStyle));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.OrderNo.Trim().Split(']')[1], _oFontStyle));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase( oPSD.RSStateInString, _oFontStyle));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.ProductName.Trim(), _oFontStyleBold));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.ColorName.Trim() + "/" + oPSD.PSBatchNo, _oFontStyle));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oPSD.ProductionQty, 2).ToString() + " KG, " + oPSD.BatchCardNo, _oFontStyle));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    _oPdfPCell = new PdfPCell(new Phrase(oPSD.Remarks.Trim(), _oFontStyleBold));
        //                                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Border = 0; oTempTable.AddCell(_oPdfPCell);
        //                                //    oTempTable.CompleteRow();
        //                                //}
        //                           // }
                                    

        //                            _oPdfPCell = new PdfPCell(oTempTable);
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);


        //                            int nIndex = nj;
        //                            nj = _oDUPSchedule.DUPScheduleList.Count;
        //                            _oDUPSchedule.DUPScheduleList.RemoveAt(nIndex);
        //                            flag = 1;


        //                        }
        //                        if ((nj == (_oDUPSchedule.DUPScheduleList.Count - 1)) && flag == 0)
        //                        {
        //                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);

        //                        }

        //                    }


        //                }

        //                //if (x > nMaxValue)
        //                //{
        //                //    for (int i = x; i <= nRows; i++)
        //                //    {

        //                //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.MinimumHeight = 85f; _oPdfPTable.AddCell(_oPdfPCell);

        //                //    }
        //                //}

        //                _oPdfPTable.CompleteRow();
        //            }
        //        }

        //    }

        //}

        //private int RetrunMaxValue(List<CapitalResource> oDyemachines, List<DUPSchedule> oPSS)
        //{
        //    int nMaxValue = 0;
        //    int nTempMax = 0;
        //    foreach (CapitalResource oDM in oDyemachines)
        //    {
        //        nTempMax = 0;
        //        foreach (DUPSchedule oPS in oPSS)
        //        {

        //            if (oDM.CRID == oPS.MachineID)
        //            {
        //                ++nTempMax;
        //                if (nTempMax > nMaxValue)
        //                {
        //                    nMaxValue = nTempMax;
        //                }
        //               // oPSS.Remove(oPS);

        //            }
        //        }
        //    }
        //    return nMaxValue;
        //}
        #endregion
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
