using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSolFinancial.Controllers.ReportController
{
    public class RptExecutionOrderUpdateStatusController : Controller
    {
        #region Declaration
        int _nType = 0;
        DateTime _dStartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        #endregion
        #region FN Execution Order Update Status Report

        public ActionResult ViewFNOrderUpdateStatus(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            var oFNOrderUpdateStatus = new List<FNOrderUpdateStatus>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            return View(oFNOrderUpdateStatus);
        }
        public ActionResult ViewFNOrderUpdateStatusV2(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            var oFNOrderUpdateStatus = new List<FNOrderUpdateStatus>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            ViewBag.BUID = buid;
            return View(oFNOrderUpdateStatus);
        }

        //[HttpPost]
        //public JsonResult GetsFNOrderUpdateStatus(FNOrderUpdateStatus oFNOrderUpdateStatus)
        //{
        //    var oFNOrderUpdateStatuss = this.GetFNOrderUpdateStatus(oFNOrderUpdateStatus.Params);
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFNOrderUpdateStatuss);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //private List<FNOrderUpdateStatus> GetFNOrderUpdateStatus(string sParam)
        //{
        //    List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();

        //    try
        //    {
        //        bool bIsDate = Convert.ToBoolean(sParam.Split('~')[0]);
        //        DateTime dtFrom = Convert.ToDateTime(sParam.Split('~')[1]);
        //        DateTime dtTo = Convert.ToDateTime(sParam.Split('~')[2]);
        //        string sFNExOID = sParam.Split('~')[3].Trim();
        //        string sBuyerID = sParam.Split('~')[4].Trim();
        //        string sFSCDID = sParam.Split('~')[5].Trim();

        //        oFNOrderUpdateStatuss = FNOrderUpdateStatus.Gets(bIsDate, dtFrom, dtTo, sFNExOID, sBuyerID, sFSCDID,((User)Session[SessionInfo.CurrentUser]).UserID);

        //    }
        //    catch (Exception ex)
        //    {
        //        oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
        //    }
        //    return oFNOrderUpdateStatuss;
        //}
        private string MakeSQL(FNOrderUpdateStatus oFNOrderUpdateStatus)
        {
            string sParams = oFNOrderUpdateStatus.ErrorMessage;
            int nBUID = 0;
            int cboTypeInDate = 0;
          
            int cboPODate = 0;
            DateTime dPOStartDate = DateTime.Today;
            DateTime dPOEndDate = DateTime.Today;
          
            string sPONo = "";
            string sContractorIDs = "", sBuyingHouseIDs = "", sMktPersonIDs = "", sMktGroupIDs="";
            string sDispoNo = "";
            _nType = 0;
            DateTime dFromTypeDateAdvSearch = DateTime.Today;
            DateTime dToTypeDateAdvSearch = DateTime.Today;
            int nStatus = 0;

            int nCount = 0;
            if (!String.IsNullOrEmpty(sParams))
            {
                nBUID = Convert.ToInt32(sParams.Split('~')[nCount++]);
                if (sParams.Split('~').Count() > nCount)
                {
                    _nType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                    cboTypeInDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                    dFromTypeDateAdvSearch = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                    dToTypeDateAdvSearch = Convert.ToDateTime(sParams.Split('~')[nCount++]);

                   
                }
                cboPODate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dPOStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dPOEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);

                sPONo = sParams.Split('~')[nCount++];
                sContractorIDs = sParams.Split('~')[nCount++];
        
                sDispoNo = sParams.Split('~')[nCount++];
                
                if (sParams.Split('~').Count() > nCount)
                    nStatus = Convert.ToInt32(sParams.Split('~')[nCount++]);
                if (sParams.Split('~').Count() > nCount)
                    sBuyingHouseIDs = sParams.Split('~')[nCount++];
                if (sParams.Split('~').Count() > nCount)
                    sMktPersonIDs = sParams.Split('~')[nCount++];
                if (sParams.Split('~').Count() > nCount)
                    sMktGroupIDs = sParams.Split('~')[nCount++];
                
            }

            //#region BUID
            //if (nBUID > 0)
            //{
            //    sReturn = sReturn + "AND BUID = " + nBUID;
            //}
            //#endregion
            //public enum EnumFabricRequestType   {       None = 0,  [Description("Hand Loom")]   HandLoom = 1, Sample = 2, Bulk = 3,Analysis = 4,CAD = 5,olor = 6,Labdip=7,  YarnSkein = 8, SampleFOC = 9,    StockSale = 10,    Local_Bulk=11,  Local_Sample=12}
            /// 
            string sSQLSubSC = "Select FabricSalesContractID from FabricSalesContract Where CurrentStatus!=" + (int)EnumFabricPOStatus .Cancel+ " and  OrderType in (" + ((int)EnumFabricRequestType.Sample).ToString() + "," + ((int)EnumFabricRequestType.Bulk).ToString() + "," + ((int)EnumFabricRequestType.SampleFOC).ToString() + "," + ((int)EnumFabricRequestType.Local_Bulk).ToString() + "," + ((int)EnumFabricRequestType.Local_Sample).ToString()+ ")";
            string sReturn = " ";
            #region SC PO

            #region PO DATE SEARCH
            DateObject.CompareDateQuery(ref sSQLSubSC, " SCDate", cboPODate, dPOStartDate, dPOEndDate);
            #endregion

            #region Contractor
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sSQLSubSC);
                sSQLSubSC = sSQLSubSC + " ContractorID IN (" + sContractorIDs + ")";
            }
            #endregion

            #region Buying house
            if (!string.IsNullOrEmpty(sBuyingHouseIDs))
            {
                Global.TagSQL(ref sSQLSubSC);
                sSQLSubSC = sSQLSubSC + " BuyerID IN (" + sBuyingHouseIDs + ")";
            }
            #endregion

            #region Mkt Account
            if (!string.IsNullOrEmpty(sMktPersonIDs))
            {
                Global.TagSQL(ref sSQLSubSC);
                sSQLSubSC = sSQLSubSC + " MktAccountID IN (" + sMktPersonIDs + ")";
            }
            #endregion

            #region Mkt Group
            if (!string.IsNullOrEmpty(sMktGroupIDs))
            {
                Global.TagSQL(ref sSQLSubSC);
                sSQLSubSC = sSQLSubSC + " MktGroupID IN (" + sMktGroupIDs + ")";
            }
            #endregion

            #region PO
            if (!string.IsNullOrEmpty(sPONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SCNo Like '%" + sPONo + "%'";
                //sReturn = sReturn + " ExeNo Like '%" + sPONo + "%'";
            }
            #endregion

            if (!string.IsNullOrEmpty(sSQLSubSC))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FabricSalesContractID in (" + sSQLSubSC + ")";

            }
            #endregion

            #region Dispo
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo Like '%" + sDispoNo + "%'";
            }
            #endregion
            #region nType
            if (_nType > 0)
            {
                 _dStartDate = dFromTypeDateAdvSearch;
                 _dEndDate = dToTypeDateAdvSearch.AddDays(1);

                if (cboTypeInDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (cboTypeInDate == (int)EnumCompareOperator.EqualTo)
                    {
                        dToTypeDateAdvSearch = dFromTypeDateAdvSearch;
                     }
                    if (_nType == 1)// Delivery From Inspection Date
                    {
                        sReturn = sReturn + "FabricSalesContractDetailID in (Select FSCDID from FNBatchQCDetail where  LockDate>='" + dFromTypeDateAdvSearch.ToString("dd MMM yyyy") + "' and LockDate<'" + dToTypeDateAdvSearch.AddDays(1).ToString("dd MMM yyyy")+"')";
                    }
                    if (_nType == 2)// Stock Receive Date
                    {
                        sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNBatchQCDetail where isnull(RcvByID,0)>0 and StoreRcvDate>='" + dFromTypeDateAdvSearch.ToString("dd MMM yyyy") + "' and StoreRcvDate<'" + dToTypeDateAdvSearch.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    if (_nType == 3)// Delivery Challan Date
                    {
                        sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FabricDeliveryChallanDetail as GG where GG.FDCID in (Select FDCID from FabricDeliveryChallan as FDC where  isnull(FDC.DisburseBy,0)<>0 and FDC.IssueDate>='" + dFromTypeDateAdvSearch.ToString("dd MMM yyyy") + "' and FDC.IssueDate<'" + dToTypeDateAdvSearch.AddDays(1).ToString("dd MMM yyyy") + "'))";
                    }
                    if (_nType ==4)//Dispo Date
                    {
                        sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FabricExecutionOrderSpecification where IssueDate>='" + dFromTypeDateAdvSearch.ToString("dd MMM yyyy") + "' and IssueDate<'" + dToTypeDateAdvSearch.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                    if (_nType == 5)//PO Date
                    {
                        sReturn = sReturn + " FabricSalesContractDetailID in (Select FabricSalesContractID from FabricSalesContract where SCDate>='" + dFromTypeDateAdvSearch.ToString("dd MMM yyyy") + "' and SCDate<'" + dToTypeDateAdvSearch.AddDays(1).ToString("dd MMM yyyy") + "')";
                    }
                  
                }
            }
            #region Status
            if (nStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nStatus == 1)// Current Stock
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where (isnull(StoreRcvQty,0)-isnull(ExcessQty,0)-isnull(DCQty,0)+isnull(RCQty,0))>0.9)";
                }
                if (nStatus == 2)// ExcessQty Stock
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where (isnull(ExcessQty,0)-isnull(ExcessDCQty,0))>0)";
                }
                if (nStatus ==3)// waiting for Greay Received
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where (isnull(OrderQty,0)-isnull(GreyRecd,0))>0)";
                }
                if (nStatus ==4)//  Greay Received but Batch not issue
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where isnull(GreyRecd,0)>0 and isnull(GreyRecd,0)>isnull(BatchQty,0))";
                }
                if (nStatus ==5)//   Batch  issue but Insfaction not received
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where isnull(BatchQty,0)>0 and isnull(BatchQty,0)-20>(isnull(GAQty,0)+isnull(GBQty,0)+isnull(GCQty,0)+isnull(GDQty,0)+isnull(RejQty,0)))";
                }
                if (nStatus ==6)//  Wating for Receive from Insfaction Bulk
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where  (isnull(StoreRcvQty,0)+0.5)<(isnull(OrderQty,0))  and  (isnull(StoreRcvQty,0)+0.5)<(isnull(GAQty,0)+isnull(GBQty,0)+isnull(GCQty,0)+isnull(GDQty,0)+isnull(RejQty,0)))";
                }
                if (nStatus == 7)//  Wating for Receive from Insfaction Bulk
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where  (isnull(StoreRcvQty,0)+0.5)>(isnull(OrderQty,0))  and  (isnull(StoreRcvQty,0)+0.5)<(isnull(GAQty,0)+isnull(GBQty,0)+isnull(GCQty,0)+isnull(GDQty,0)+isnull(RejQty,0)))";
                }
                   if (nStatus ==8)//  Wating for Delivery
                {
                    sReturn = sReturn + " FabricSalesContractDetailID in (Select FSCDID from FNFSCToDC where ((isnull(StoreRcvQty,0)))>0 and (isnull(OrderQty,0)+isnull(RCQty,0)-5)>(isnull(DCQty,0)))";
                }
            }
             #endregion


            #endregion

            return sReturn;
        }
        public JsonResult AdvanceSearch(FNOrderUpdateStatus objFNOrderUpdateStatus)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            try
            {
                string sSQL = MakeSQL(objFNOrderUpdateStatus);
                oFNOrderUpdateStatuss = FNOrderUpdateStatus.Gets(sSQL, _nType, _dStartDate, _dEndDate, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
            }
            var jSonResult = Json(oFNOrderUpdateStatuss, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        

        public void ExcelFNOrderUpdateStatus(string sParam)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            oFNOrderUpdateStatus.ErrorMessage = sParam;
            string sSQL = MakeSQL(oFNOrderUpdateStatus);
            oFNOrderUpdateStatuss = FNOrderUpdateStatus.Gets(sSQL,_nType,_dStartDate,_dEndDate, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int rowIndex = 2;
            int nMaxColumn = 11;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Grey Receive Report");
                sheet.Name = "Grey Receive Report";

                #region Coloums


                string[] columnHead = new string[] { "SL", "PO No", "Customer", "Garments Name", "Dispo No", "Construction", "Color", "Order Qty", "Grey Recd", "Batch Qty", "Grade-A", "Grade-B", "Grade-C", "Grade-D", "Reject + Cust Pcs", "Total Ins. Qty", "Store Recd Qty", "Waiting For Recd", "Delivery Qty", "Return Qty", "Balance Qty", "Store Recd(day)", "Delivery Qty(day)", "Stock In Hand", "Excess Qty", "Remarks" };
                int[] colWidth = new int[] { 8, 18, 25, 25, 18, 18, 18, 18, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 25 };

                colIndex = 2;
                for (int i = 0; i < colWidth.Length; i++)
                {
                    sheet.Column(colIndex).Width = colWidth[i];
                    colIndex++;
                    
                }
                nMaxColumn = colIndex;
                #endregion

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                cell.Value = "Exe. Order Production Update Report"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Date

                int cboStockInDate = 0;
                DateTime dQCStartDate = DateTime.Today;
                DateTime dQCEndDate = DateTime.Today;

                int cboBatchIssueDate = 0;
                DateTime dBatchIssueDateStartDate = DateTime.Today;
                DateTime dBatchIssueDateEndDate = DateTime.Today;

                int cboPODate = 0;
                DateTime dPOStartDate = DateTime.Today;
                DateTime dPOEndDate = DateTime.Today;

                int nCount = 1;
                if (!String.IsNullOrEmpty(sParam))
                {
                    int cboType = Convert.ToInt32(sParam.Split('~')[1]);

                    if (cboType == 1)
                    {
                        cboBatchIssueDate = Convert.ToInt32(sParam.Split('~')[2]);
                        dBatchIssueDateStartDate = Convert.ToDateTime(sParam.Split('~')[3]);
                        dBatchIssueDateEndDate = Convert.ToDateTime(sParam.Split('~')[4]);
                    }
                    if (cboType == 2)
                    {
                        cboStockInDate = Convert.ToInt32(sParam.Split('~')[2]);
                        dQCStartDate = Convert.ToDateTime(sParam.Split('~')[3]);
                        dQCEndDate = Convert.ToDateTime(sParam.Split('~')[4]);
                    }
                    cboPODate = Convert.ToInt32(sParam.Split('~')[5]);
                    dPOStartDate = Convert.ToDateTime(sParam.Split('~')[6]);
                    dPOEndDate = Convert.ToDateTime(sParam.Split('~')[7]);

                }
                string sDateString = "";
                //-------------------Stock Receive Date----------------------//
                if (cboStockInDate == (int)EnumCompareOperator.EqualTo)
                {
                    sDateString = "Stock Receive Date Equals " + dQCStartDate.ToString("dd MMM yyyy");
                }
                if (cboStockInDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sDateString = "Stock Receive Date Not Equals " + dQCStartDate.ToString("dd MMM yyyy");
                } if (cboStockInDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sDateString = "Stock Receive Date Is Grater Than " + dQCStartDate.ToString("dd MMM yyyy");
                } if (cboStockInDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sDateString = "Stock Receive Date Is Smaller Than: " + dQCStartDate.ToString("dd MMM yyyy");
                } if (cboStockInDate == (int)EnumCompareOperator.Between)
                {
                    sDateString = "Stock Receive Date Is Between : " + dQCStartDate.ToString("dd MMM yyyy") + " And " + dQCEndDate.ToString("dd MMM yyyy");
                } if (cboStockInDate == (int)EnumCompareOperator.NotBetween)
                {
                    sDateString = "Stock Receive Date Is Not Between: " + dQCStartDate.ToString("dd MMM yyyy") + " And " + dQCEndDate.ToString("dd MMM yyyy");
                }
                if (sDateString != "") sDateString = sDateString + ", ";
                //-------------------Batch Issue Date----------------------//

                if (cboBatchIssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    sDateString = sDateString + " Batch Issue Date Equals " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
                }
                if (cboBatchIssueDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sDateString = sDateString + " Batch Issue Date Not Equals " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
                } if (cboBatchIssueDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sDateString = sDateString + " Batch Issue Date Is Grater Than " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
                } if (cboBatchIssueDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sDateString = sDateString + " Batch Issue Date Is Smaller Than: " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
                } if (cboBatchIssueDate == (int)EnumCompareOperator.Between)
                {
                    sDateString = sDateString + " Batch Issue Date Is Between : " + dBatchIssueDateStartDate.ToString("dd MMM yyyy") + " And " + dBatchIssueDateEndDate.ToString("dd MMM yyyy");
                } if (cboBatchIssueDate == (int)EnumCompareOperator.NotBetween)
                {
                    sDateString = sDateString + " Batch Issue Date Is Not Between: " + dBatchIssueDateStartDate.ToString("dd MMM yyyy") + " And " + dBatchIssueDateEndDate.ToString("dd MMM yyyy");
                }
                //-------------------PO Date----------------------//
                if (sDateString != "") sDateString = sDateString + ", ";

                if (cboPODate == (int)EnumCompareOperator.EqualTo)
                {
                    sDateString = sDateString + ", PO Date Equals " + dPOStartDate.ToString("dd MMM yyyy");
                }
                if (cboPODate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sDateString = sDateString + ", PO Date Not Equals " + dPOStartDate.ToString("dd MMM yyyy");
                } if (cboPODate == (int)EnumCompareOperator.GreaterThan)
                {
                    sDateString = sDateString + ", PO Date Is Grater Than " + dPOStartDate.ToString("dd MMM yyyy");
                } if (cboPODate == (int)EnumCompareOperator.SmallerThan)
                {
                    sDateString = sDateString + ", PO Date Is Smaller Than: " + dPOStartDate.ToString("dd MMM yyyy");
                } if (cboPODate == (int)EnumCompareOperator.Between)
                {
                    sDateString = sDateString + ", PO Date Is Between : " + dPOStartDate.ToString("dd MMM yyyy") + " And " + dPOEndDate.ToString("dd MMM yyyy");
                } if (cboPODate == (int)EnumCompareOperator.NotBetween)
                {
                    sDateString = sDateString + ", PO Date Is Not Between: " + dPOStartDate.ToString("dd MMM yyyy") + " And " + dPOEndDate.ToString("dd MMM yyyy");
                }

                //cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                cell.Value = sDateString; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Column Header
                colIndex = 2;

                for (int i = 0; i < columnHead.Length; i++)
                {
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    colIndex++;
                }

                rowIndex++;
                #endregion

                #region Body

                if (oFNOrderUpdateStatuss.Any())
                {
                    int count = 0;
                    foreach (FNOrderUpdateStatus oItem in oFNOrderUpdateStatuss)
                    {
                        colIndex = 2;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (++count).ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SCNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ColorInfo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GreyRecd; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BatchQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GradeAQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GradeBQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GradeCQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GradeDQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RejQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StoreRcvQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WForRcvQtyInCalST; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DCQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RCQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BalanceQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StoreRcvQtyDay; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DCQtyDay; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StockInHand; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ExcessQty; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rowIndex++;
                    }

                    #region Summary
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 8]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    colIndex = 8;
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.OrderQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.GreyRecd); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.BatchQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.GradeAQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.GradeBQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.GradeCQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.GradeDQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.RejQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.TotalQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.StoreRcvQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.WForRcvQtyInCalST); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.DCQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.RCQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.BalanceQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.StoreRcvQtyDay); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.DCQtyDay); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.StockInHand); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oFNOrderUpdateStatuss.Sum(x => x.ExcessQty); cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowIndex++;
                    #endregion
                }
                else
                {
                    for (int i = 0; i < columnHead.Length; i++)
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex++;
                    }
                    rowIndex++;
                }

                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FN Exe Order Update Status.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult PrintFNOrderUpdateStatus(string sParam)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            string _sParam = sParam;
            oFNOrderUpdateStatus.ErrorMessage = sParam;
            try
            {
                
                 string sSQL = MakeSQL(oFNOrderUpdateStatus);
                 oFNOrderUpdateStatuss = FNOrderUpdateStatus.Gets(sSQL, _nType, _dStartDate, _dEndDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch(Exception ex){
                oFNOrderUpdateStatus = new FNOrderUpdateStatus();
                oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);

            }

            #region Date

            int cboStockInDate = 0;
            DateTime dQCStartDate = DateTime.Today;
            DateTime dQCEndDate = DateTime.Today;

            int cboBatchIssueDate = 0;
            DateTime dBatchIssueDateStartDate = DateTime.Today;
            DateTime dBatchIssueDateEndDate = DateTime.Today;

            int cboPODate = 0;
            DateTime dPOStartDate = DateTime.Today;
            DateTime dPOEndDate = DateTime.Today;

            int nCount = 1;
            if (!String.IsNullOrEmpty(sParam))
            {
                int cboType = Convert.ToInt32(sParam.Split('~')[1]);

                if (cboType == 1)
                {
                    cboBatchIssueDate = Convert.ToInt32(sParam.Split('~')[2]);
                    dBatchIssueDateStartDate = Convert.ToDateTime(sParam.Split('~')[3]);
                    dBatchIssueDateEndDate = Convert.ToDateTime(sParam.Split('~')[4]);
                }
                if (cboType == 2)
                {
                    cboStockInDate = Convert.ToInt32(sParam.Split('~')[2]);
                    dQCStartDate = Convert.ToDateTime(sParam.Split('~')[3]);
                    dQCEndDate = Convert.ToDateTime(sParam.Split('~')[4]);
                }
                cboPODate = Convert.ToInt32(sParam.Split('~')[5]);
                dPOStartDate = Convert.ToDateTime(sParam.Split('~')[6]);
                dPOEndDate = Convert.ToDateTime(sParam.Split('~')[7]);

            }
            string sDateString = "";
            //-------------------Stock Receive Date----------------------//
            if (cboStockInDate == (int)EnumCompareOperator.EqualTo)
            {
                sDateString = "Stock Receive Date Equals " + dQCStartDate.ToString("dd MMM yyyy");
            }
            if (cboStockInDate == (int)EnumCompareOperator.NotEqualTo)
            {
                sDateString = "Stock Receive Date Not Equals " + dQCStartDate.ToString("dd MMM yyyy");
            } if (cboStockInDate == (int)EnumCompareOperator.GreaterThan)
            {
                sDateString = "Stock Receive Date Is Grater Than " + dQCStartDate.ToString("dd MMM yyyy");
            } if (cboStockInDate == (int)EnumCompareOperator.SmallerThan)
            {
                sDateString = "Stock Receive Date Is Smaller Than: " + dQCStartDate.ToString("dd MMM yyyy");
            } if (cboStockInDate == (int)EnumCompareOperator.Between)
            {
                sDateString = "Stock Receive Date Is Between : " + dQCStartDate.ToString("dd MMM yyyy") + " And " + dQCEndDate.ToString("dd MMM yyyy");
            } if (cboStockInDate == (int)EnumCompareOperator.NotBetween)
            {
                sDateString = "Stock Receive Date Is Not Between: " + dQCStartDate.ToString("dd MMM yyyy") + " And " + dQCEndDate.ToString("dd MMM yyyy");
            }
            if (sDateString != "") sDateString = sDateString + ", ";
            //-------------------Batch Issue Date----------------------//

            if (cboBatchIssueDate == (int)EnumCompareOperator.EqualTo)
            {
                sDateString = sDateString + " Batch Issue Date Equals " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
            }
            if (cboBatchIssueDate == (int)EnumCompareOperator.NotEqualTo)
            {
                sDateString = sDateString + " Batch Issue Date Not Equals " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
            } if (cboBatchIssueDate == (int)EnumCompareOperator.GreaterThan)
            {
                sDateString = sDateString + " Batch Issue Date Is Grater Than " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
            } if (cboBatchIssueDate == (int)EnumCompareOperator.SmallerThan)
            {
                sDateString = sDateString + " Batch Issue Date Is Smaller Than: " + dBatchIssueDateStartDate.ToString("dd MMM yyyy");
            } if (cboBatchIssueDate == (int)EnumCompareOperator.Between)
            {
                sDateString = sDateString + " Batch Issue Date Is Between : " + dBatchIssueDateStartDate.ToString("dd MMM yyyy") + " And " + dBatchIssueDateEndDate.ToString("dd MMM yyyy");
            } if (cboBatchIssueDate == (int)EnumCompareOperator.NotBetween)
            {
                sDateString = sDateString + " Batch Issue Date Is Not Between: " + dBatchIssueDateStartDate.ToString("dd MMM yyyy") + " And " + dBatchIssueDateEndDate.ToString("dd MMM yyyy");
            }
            //-------------------PO Date----------------------//
            if (sDateString != "") sDateString = sDateString + ", ";

            if (cboPODate == (int)EnumCompareOperator.EqualTo)
            {
                sDateString = sDateString + ", PO Date Equals " + dPOStartDate.ToString("dd MMM yyyy");
            }
            if (cboPODate == (int)EnumCompareOperator.NotEqualTo)
            {
                sDateString = sDateString + ", PO Date Not Equals " + dPOStartDate.ToString("dd MMM yyyy");
            } if (cboPODate == (int)EnumCompareOperator.GreaterThan)
            {
                sDateString = sDateString + ", PO Date Is Grater Than " + dPOStartDate.ToString("dd MMM yyyy");
            } if (cboPODate == (int)EnumCompareOperator.SmallerThan)
            {
                sDateString = sDateString + ", PO Date Is Smaller Than: " + dPOStartDate.ToString("dd MMM yyyy");
            } if (cboPODate == (int)EnumCompareOperator.Between)
            {
                sDateString = sDateString + ", PO Date Is Between : " + dPOStartDate.ToString("dd MMM yyyy") + " And " + dPOEndDate.ToString("dd MMM yyyy");
            } if (cboPODate == (int)EnumCompareOperator.NotBetween)
            {
                sDateString = sDateString + ", PO Date Is Not Between: " + dPOStartDate.ToString("dd MMM yyyy") + " And " + dPOEndDate.ToString("dd MMM yyyy");
            }
            #endregion
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            int nBUID = Convert.ToInt32(_sParam.Split('~')[0]);
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptPrintFNOrderUpdateStatus oReport = new rptPrintFNOrderUpdateStatus();
            byte[] abytes = oReport.PrepareReport(oFNOrderUpdateStatuss, oCompany, sDateString);
            return File(abytes, "application/pdf");
        }


        #endregion

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #region Stock Report
        public ActionResult ViewFNOrderUpdateStatusFabricStock(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            var oFNOrderUpdateStatus = new List<FNOrderUpdateStatus>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.FabricDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(oFNOrderUpdateStatus);
        }
        [HttpPost]
        public JsonResult AdvSearch(FNOrderUpdateStatus oFNOrderUpdateStatus)
        {
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            int nWorkingUnitID = 0;
            try
            {
                int nCount = 0;
                if (!string.IsNullOrEmpty(oFNOrderUpdateStatus.Params))
                {
                    int nReportType = Convert.ToInt32(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    DateTime dStartDate = Convert.ToDateTime(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    DateTime dEndDate = Convert.ToDateTime(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    nWorkingUnitID = Convert.ToInt32(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    oFNOrderUpdateStatuss = FNOrderUpdateStatus.GetStockReport(dStartDate, dEndDate.AddDays(1), nReportType, nWorkingUnitID,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
            }

            var jSonResult = Json(oFNOrderUpdateStatuss, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        public void ExportToExcel(string sParams)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            int nReportType = 0;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            int nWorkingUnitID = 0;
            try
            {
                int nCount = 0;
                if (!string.IsNullOrEmpty(sParams))
                {
                    nReportType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                    dStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                    dEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                 //   nWorkingUnitID = Convert.ToInt32(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    oFNOrderUpdateStatuss = FNOrderUpdateStatus.GetStockReport(dStartDate, dEndDate, nReportType, nWorkingUnitID,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right, IsBold=true });
            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Buyer Name", Width = 35f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Garments", Width = 35f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Construction", Width = 40f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Design", Width = 15f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Width", Width = 15f, IsRotate = false, Align = TextAlign.Left, IsBold = true });
            table_header.Add(new TableHeader { Header = "Color", Width = 30f, IsRotate = false, Align = TextAlign.Left, IsBold = true });

            table_header.Add(new TableHeader { Header = "Order Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Total Delivery", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            table_header.Add(new TableHeader { Header = "Yet To Delivery", Width = 15f, IsRotate = false, Align = TextAlign.Right, IsBold = true });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Stock Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Stock Report");
                sheet.Name = "Stock Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Stock Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Date
                cell = sheet.Cells[++nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date : " + dStartDate.ToString("dd MMM yyyy") + " to " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;


                EnumFabricRequestType eOrderType = 0;
                List<FNOrderUpdateStatus> oLoops = oFNOrderUpdateStatuss.GroupBy(x => x.OrderType).Select(x => x.First()).ToList();
                List<FNOrderUpdateStatus> oTempFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();

                foreach (FNOrderUpdateStatus oItem in oLoops)
                {
                    oTempFNOrderUpdateStatuss = oFNOrderUpdateStatuss.Where(x => x.OrderType == oItem.OrderType).ToList();
                    if (oTempFNOrderUpdateStatuss.Count > 0)
                    {
                        string sHeadName = oItem.OrderType.ToString();
                        eOrderType = (EnumFabricRequestType)oItem.OrderType;

                        ExcelTool.FillCellMerge(ref sheet, eOrderType.ToString(), nRowIndex, nRowIndex++, 2, nEndCol - 1, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Center, true);
                        ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 11, true, true);
                        foreach (var obj in oTempFNOrderUpdateStatuss)
                        {
                            nStartCol = 2;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.PINo, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.WeaveName, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FabricWidth, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Color, false, ExcelHorizontalAlignment.Left, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.OrderQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DeliveryQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyOut, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YetToDelivery, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            nSL++;
                            nRowIndex++;
                        }
                        #region total
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Total", nRowIndex, nRowIndex, 2, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, 11, Math.Round(oTempFNOrderUpdateStatuss.Sum(x => x.OrderQty), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oTempFNOrderUpdateStatuss.Sum(x => x.DeliveryQty), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, 13, Math.Round(oTempFNOrderUpdateStatuss.Sum(x => x.QtyOut), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, 14, Math.Round(oTempFNOrderUpdateStatuss.Sum(x => x.YetToDelivery), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        nRowIndex++;
                        nRowIndex++;
                        #endregion
                    }
                }
                #region total
                nRowIndex--;
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, 2, 10, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, true);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 11, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.OrderQty), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.DeliveryQty), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 13, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.QtyOut), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 14, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.YetToDelivery), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                #endregion

                nRowIndex = nRowIndex + 4;
                nStartCol = 2;

                ExcelTool.FillCellMerge(ref sheet, "Delivered Qty " + " (" + dStartDate.ToString("dd MMM yyyy") + " - " + dEndDate.ToString("dd MMM yyyy") + ")", nRowIndex, nRowIndex++, 6, 9, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, true);
                foreach (FNOrderUpdateStatus obj in oLoops)
                {
                    oTempFNOrderUpdateStatuss = oFNOrderUpdateStatuss.Where(x => x.OrderType == obj.OrderType).ToList();
                    if (oTempFNOrderUpdateStatuss.Count > 0)
                    {
                        eOrderType = (EnumFabricRequestType)obj.OrderType;
                        ExcelTool.FillCellMerge(ref sheet, eOrderType.ToString(), nRowIndex, nRowIndex, 6, 7, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                        ExcelTool.FillCellMerge(ref sheet, Math.Round(oFNOrderUpdateStatuss.Where(x => x.OrderType == obj.OrderType).Sum(x => x.QtyOut), 2).ToString(), nRowIndex, nRowIndex++, 8, 9, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                    }
                }
                ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, 6, 7, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellMerge(ref sheet, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.QtyOut), 2).ToString(), nRowIndex, nRowIndex++, 8, 9, true, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Center, false);

                #endregion
                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Stock_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public void PrintStockPosition(string sParams)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            int nReportType = 0;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            int nWorkingUnitID = 0;
            try
            {
                int nCount = 0;
                if (!string.IsNullOrEmpty(sParams))
                {
                    nReportType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                    dStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                    dEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                   // nWorkingUnitID = Convert.ToInt32(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    oFNOrderUpdateStatuss = FNOrderUpdateStatus.GetStockReport(dStartDate, dEndDate, nReportType, nWorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "PO Qty", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Account Holder", Width = 35f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Customer", Width = 40f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Construction", Width = 35f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Opening", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Received", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });

            table_header.Add(new TableHeader { Header = "Issued", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Stock Roll", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Location", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Order Type", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Rcv Date", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });

            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Daily Delivery Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Daily Delivery Report");
                sheet.Name = "Daily Delivery Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol - 5]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol - 5]; cell.Merge = true;
                cell.Value = "Daily Delivery Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol - 5]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Date
                cell = sheet.Cells[++nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                cell.Value = "Production Date : " + dStartDate.ToString("dd MMM yyyy") + " to " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                #endregion

                #region Date
                cell = sheet.Cells[nRowIndex, 9, nRowIndex, 15]; cell.Merge = true;
                cell.Value = "Reporting Date : " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 11, true, true);
                foreach (FNOrderUpdateStatus obj in oFNOrderUpdateStatuss)
                {
                    nStartCol = 2;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Center, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.OrderQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction, false, ExcelHorizontalAlignment.Left, false, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyOpen, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyIn, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.QtyOut, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.StockBalance, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.RollNo, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Location, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.OrderName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "-", false, ExcelHorizontalAlignment.Left, false, false);
                    nSL++;
                    nRowIndex++;
                            
                 }
                #region total
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "G. Total", nRowIndex, nRowIndex, 2, 8, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 9, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.QtyOpen), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 10, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.QtyIn), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 11, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.QtyOut), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, 12, Math.Round(oFNOrderUpdateStatuss.Sum(x => x.StockBalance), 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, 13, nEndCol-1, true, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Center, false);
                nRowIndex++;
                #endregion

                #endregion

                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Godown_Stock_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public ActionResult PrintPDF(string sParams)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            int nReportType = 0;
            DateTime dStartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            int nWorkingUnitID = 0;
            try
            {
                int nCount = 0;
                if (!string.IsNullOrEmpty(sParams))
                {
                    nReportType = Convert.ToInt32(sParams.Split('~')[nCount++]);
                    dStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                    dEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                 //   nWorkingUnitID = Convert.ToInt32(oFNOrderUpdateStatus.Params.Split('~')[nCount++]);
                    oFNOrderUpdateStatuss = FNOrderUpdateStatus.GetStockReport(dStartDate, dEndDate, nReportType, nWorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

           string sUserName= ((User)Session[SessionInfo.CurrentUser]).UserName;

            rptStockReport oReport = new rptStockReport();
            byte[] abytes = oReport.PrepareReport(oFNOrderUpdateStatuss, oCompany, oBusinessUnit, dStartDate, dEndDate, sUserName);
            return File(abytes, "application/pdf");
        }
      
        #endregion

        #region FNBatch QC Detail
        [HttpPost]
        public JsonResult GetFNBatchQCDetail(FNBatchQCDetail oFNBatchQCDetail)
        {
            FNBatchQCDetail _oFNBatchQCDetail = new FNBatchQCDetail();
            List<FNBatchQCDetail> _oFNBatchQCDetails = new List<FNBatchQCDetail>();
            try
            {
                String SQL = "SELECT * FROM View_FNBatchQCDetail AS HH WHERE HH.FSCDID = " + oFNBatchQCDetail.FSCDID + " Order By FSCDID ASC";
                _oFNBatchQCDetails = FNBatchQCDetail.Gets(SQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNBatchQCDetail = new FNBatchQCDetail();
                _oFNBatchQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNBatchQCDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult PrintStockLedger(int nID)
        {
            string sIDs = "";
            FNBatchQCDetail oFNBatchQCDetail = new FNBatchQCDetail();
            List<FNBatchQCDetail> oFNBatchQCDetails = new List<FNBatchQCDetail>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            List<FabricReturnChallan> oFabricReturnChallans = new List<FabricReturnChallan>();
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            if (nID > 0)
            {
                oFabricSCReport = oFabricSCReport.Get(nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNBatchQCDetails = FNBatchQCDetail.Gets("SELECT * FROM FNBatchQCDetail WHERE  FSCDID = " + oFabricSCReport.FabricSalesContractDetailID + " order by StoreRcvDate Asc", ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFDCRegisters = FDCRegister.Gets_FDC("where  FSCDID = " + oFabricSCReport.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricReturnChallanDetails = FabricReturnChallanDetail.Gets("Select * from View_FabricReturnChallanDetail  where FSCDID in ("+ oFabricSCReport.FabricSalesContractDetailID+")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                sIDs = string.Join(",", oFabricReturnChallanDetails.Select(x => x.FabricReturnChallanID).Distinct().ToList());
                oFabricReturnChallans = FabricReturnChallan.Gets("Select * from View_FabricReturnChallan where FabricReturnChallanID  in (" + sIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFabricReturnChallanDetails.ForEach(x =>
                    {
                        if (oFabricReturnChallans.FirstOrDefault() != null && oFabricReturnChallans.FirstOrDefault().FabricReturnChallanID > 0 && oFabricReturnChallans.Where(b => (b.FabricReturnChallanID == x.FabricReturnChallanID)).Count() > 0)
                        {
                            x.ChallanNo = oFabricReturnChallans.Where(p => (p.FabricReturnChallanID == x.FabricReturnChallanID) && p.FabricReturnChallanID > 0).FirstOrDefault().ReturnNo;
                            x.ReturnDate = oFabricReturnChallans.Where(p => (p.FabricReturnChallanID == x.FabricReturnChallanID) && p.FabricReturnChallanID > 0).FirstOrDefault().ReturnDate;
                        }
                    });
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFNBatchDetail er = new rptFNBatchDetail();
            rptStockLedger oReport = new rptStockLedger();
            byte[] abytes = oReport.PrepareReport( oFNBatchQCDetails, oFabricSCReport, oFDCRegisters,oFabricReturnChallanDetails, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintStatement(int nFSCDID, int nBUID)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();
            if (nFSCDID > 0)
            {
                oFSCD = oFSCD.Get(nFSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM VIEW_FNBatch AS EE WHERE EE.FNExOID = " + nFSCDID;
                oFNBatchs = FNBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFNBatchs.Count > 0)
            {
                oFNBatchs.ForEach(x => {
                    x.FNBatchCards = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard AS EE WHERE EE.FNBatchID = " + x.FNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    x.FNProductionBatchs = FNProductionBatch.Gets("SELECT * FROm View_FNProductionBatch WHERE FNBatchID="+x.FNBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            });
                
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nBUID > 0)
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptFNOrderUpdateStatusStatement oReport = new rptFNOrderUpdateStatusStatement();
                byte[] abytes = oReport.PrepareReport(oFSCD, oFNBatchs, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }

        }

    }
}