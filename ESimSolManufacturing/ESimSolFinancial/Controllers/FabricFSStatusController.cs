using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSolFinancial.Controllers
{
    public class FabricFSStatusController :Controller
    {
        #region DECLARATION
        FabricFSStatus _oFabricFSStatus = new FabricFSStatus();
        List<FabricFSStatus> _oFabricFSStatuss = new List<FabricFSStatus>();
        FNOrderUpdateStatus _oFNOrderUpdateStatus = new FNOrderUpdateStatus();
        List<FNOrderUpdateStatus> _oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
        #endregion

        [HttpGet]
        public ActionResult ViewFabricFSStatus(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricFSStatuss = new List<FabricFSStatus>();
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            oFabricOrderSetups = FabricOrderSetup.GetsActive(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = oFabricOrderSetups.Where(o => o.IsLocal == false).ToList();
            ViewBag.FabricPOStatus = EnumObject.jGets(typeof(EnumFabricPOStatus));          
            return View(_oFabricFSStatuss);
        }

        [HttpGet]
        public ActionResult ViewFabricFSStatusDetail(int FSCID)
        {
            _oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            _oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            String Status = "";
            try
            {                
                String sSQL = " AND FabricSalesContractID = " + FSCID;
                _oFNOrderUpdateStatuss = FNOrderUpdateStatus.Gets(sSQL, 0, DateTime.Now, DateTime.Now, (int)Session[SessionInfo.currentUserID]);  
                if(_oFNOrderUpdateStatuss.Count>0){
                    Status = "PO No: " + _oFNOrderUpdateStatuss[0].SCNo + ", Buyer: " + _oFNOrderUpdateStatuss[0].BuyerName + ", Garments: " + _oFNOrderUpdateStatuss[0].ContractorName;
                }

                ViewBag.Status = Status;
                
            }
            catch (Exception ex)
            {
                _oFNOrderUpdateStatus = new FNOrderUpdateStatus();
                _oFNOrderUpdateStatus.ErrorMessage = ex.Message;
                _oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
                _oFNOrderUpdateStatuss.Add(_oFNOrderUpdateStatus);
            }
            return View(_oFNOrderUpdateStatuss);
        }

        [HttpPost]
        public JsonResult AdvanchSearch(FabricFSStatus oFabricFSStatus)
        {
            _oFabricFSStatuss = new List<FabricFSStatus>();
           
            try
            {
                string sSQL = GetSQL(oFabricFSStatus.ErrorMessage);
                int nReportType = Convert.ToInt32(oFabricFSStatus.ErrorMessage.Split('~')[17]);
                _oFabricFSStatuss = FabricFSStatus.GetsFabricFSStatus(sSQL,nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFabricFSStatus = new FabricFSStatus();
                _oFabricFSStatus.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oFabricFSStatuss, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQL(string sTemp)
        {
            string sReturn1 = " ";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sTemp))
            {
                #region Set Values
                string sExportPINo = Convert.ToString(sTemp.Split('~')[0]);
                string sFabricID = Convert.ToString(sTemp.Split('~')[1]);
                string sContractorIDs = Convert.ToString(sTemp.Split('~')[2]);
                string sBuyerIDs = Convert.ToString(sTemp.Split('~')[3]);
                string sMktPersonIDs = Convert.ToString(sTemp.Split('~')[4]);


                int nCboSCDate = Convert.ToInt32(sTemp.Split('~')[5]);
                DateTime dFromSCDate = DateTime.Now;
                DateTime dToSCDate = DateTime.Now;
                if (nCboSCDate > 0)
                {
                    dFromSCDate = Convert.ToDateTime(sTemp.Split('~')[6]);
                    dToSCDate = Convert.ToDateTime(sTemp.Split('~')[7]);
                }

                int ncboApproveDate = Convert.ToInt32(sTemp.Split('~')[8]);
                DateTime dFromApproveDate = DateTime.Now;
                DateTime dToApproveDate = DateTime.Now;
                if (ncboApproveDate > 0)
                {
                    dFromApproveDate = Convert.ToDateTime(sTemp.Split('~')[9]);
                    dToApproveDate = Convert.ToDateTime(sTemp.Split('~')[10]);
                }

                int ncboProductionType = Convert.ToInt32(sTemp.Split('~')[11]);
                string sOrderTypeIDs = Convert.ToString(sTemp.Split('~')[12]);
                string sCurrentStatus = Convert.ToString(sTemp.Split('~')[13]);
                int nIsPrinting = Convert.ToInt32(sTemp.Split('~')[14]);
                string sMKTGroupIDs = Convert.ToString(sTemp.Split('~')[15]);
                int nBUID = Convert.ToInt32(sTemp.Split('~')[16]);
                string sPONo = Convert.ToString(sTemp.Split('~')[18]);
                int IsWithClose = Convert.ToInt32(sTemp.Split('~')[19]);

                #endregion

                #region Make Query
                #region Export PI NO
                if (!string.IsNullOrEmpty(sExportPINo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SCNo LIKE '%" + sExportPINo + "%'";
                }
                #endregion

                //#region sFabricID
                //if (!string.IsNullOrEmpty(sFabricID))
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "Select * from FabricSalesContract where FabricSalesContractID in (Select FabricSalesContractDetail.FabricSalesContractID from FabricSalesContractDetail where FabricID in (Select Fabric.FabricID from Fabric where FabricNo like '%" + sFabricID + "%'))";
                //}
                //#endregion

                #region ContractorID
                if (!String.IsNullOrEmpty(sContractorIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ContractorID in( " + sContractorIDs + ") ";
                }
                #endregion

                #region BBuyerID
                if (!String.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID in( " + sBuyerIDs + ") ";
                }
                #endregion

                #region sMAccountIDs
                if (!String.IsNullOrEmpty(sMktPersonIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID in(" + sMktPersonIDs + ") ";
                }
                #endregion

                //#region sMKTGroupIDs
                //if (!String.IsNullOrEmpty(sMKTGroupIDs))
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " MktAccountID in(Select MarketingAccountID  from View_MarketingAccount where GroupID >0 and GroupID in (" + sMKTGroupIDs + ")) ";
                //}
                //#endregion

                #region F SC Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(";
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion
                #region Revise Date
                if (nCboSCDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn,true);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboSCDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    sReturn = sReturn + ")";
                }
                #endregion

                #region Approval Date
                if (ncboApproveDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (ncboApproveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (ncboApproveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromApproveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToApproveDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                //#region ncboProductionType
                //if (ncboProductionType > 0)
                //{

                //    if (ncboProductionType == 1)
                //    {
                //        Global.TagSQL(ref sReturn);
                //        sReturn = sReturn + " IsInHouse =1";
                //    }
                //    else if (ncboProductionType == 2)
                //    {
                //        Global.TagSQL(ref sReturn);
                //        sReturn = sReturn + " IsInHouse =0";
                //    }

                //}
                //#endregion
                //#region nIsPrinting
                //if (nIsPrinting > 0)
                //{
                //    if (nIsPrinting == 1)
                //    {
                //        Global.TagSQL(ref sReturn);
                //        sReturn = sReturn + " IsPrint =1";
                //    }
                //    else if (nIsPrinting == 2)
                //    {
                //        Global.TagSQL(ref sReturn);
                //        sReturn = sReturn + "IsPrint =0";
                //    }
                //}
                //#endregion

                #region OrderType
                if (!string.IsNullOrEmpty(sOrderTypeIDs))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderType IN (" + sOrderTypeIDs + ") ";
                }
                #endregion
                //#region CurrentStatus
                //if (!string.IsNullOrEmpty(sCurrentStatus))
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + " Currentstatus IN (" + sCurrentStatus + ") ";
                //}
                //#endregion

                List<MarketingAccount> oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(nBUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " MktAccountID IN (Select MarketingAccount.MarketingAccountID from MarketingAccount where GroupID>0 and GroupID in (Select GroupID from MarketingAccount where UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ") or UserID =" + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
                }
                #endregion

                #region PONO
                if (!string.IsNullOrEmpty(sPONo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " SCNo LIKE '%" + sPONo + "%'";
                }
                #endregion

                #region With Close
                if (IsWithClose == 1)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboSCDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + "FabricSalesContractID IN (SELECT FabricSCID FROM FabricSCHistory WHERE CurrentStatus = 5 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                    if (nCboSCDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + "FabricSalesContractID IN (SELECT FabricSCID FROM FabricSCHistory WHERE CurrentStatus = 5 AND CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSCDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSCDate.ToString("dd MMM yyyy") + "',106)))";
                    }
                    
                }
                #endregion

            }
            sReturn = sReturn1 + sReturn + " order by SCDate DESC";
            return sReturn;
        }
        public ActionResult FabricFSStatusPreview(string sValue, double nts)
        {
            _oFabricFSStatuss = new List<FabricFSStatus>();
            _oFabricFSStatus = new FabricFSStatus();
            int nReportType = Convert.ToInt32(sValue.Split('~')[17]);
            string sHeader = "";
            int nDate = 0;
            DateTime sStartDate = DateTime.MinValue;
            DateTime sEndTime = DateTime.MinValue;

            #region SET PO date RANGE
            nDate = Convert.ToInt32(sValue.Split('~')[5]);
            sStartDate = Convert.ToDateTime(sValue.Split('~')[6]);
            sEndTime = Convert.ToDateTime(sValue.Split('~')[7]);
            if (nDate == (int)EnumCompareOperator.EqualTo)
            {
                sHeader = "PO Date " + sStartDate.ToString("dd MMM yyyy");
            }
            if (nDate == (int)EnumCompareOperator.Between)
            {
                sHeader = "PO Date " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndTime.ToString("dd MMM yyyy");
            }
            #endregion
            #region SET Approve date RANGE
            nDate = Convert.ToInt32(sValue.Split('~')[8]);
            sStartDate = Convert.ToDateTime(sValue.Split('~')[9]);
            sEndTime = Convert.ToDateTime(sValue.Split('~')[10]);
            if (nDate == (int)EnumCompareOperator.EqualTo)
            {
                if (String.IsNullOrEmpty(sHeader))
                {
                    sHeader = "Approve Date " + sStartDate.ToString("dd MMM yyyy");
                }
                else
                {
                    sHeader = " AND Approve Date " + sStartDate.ToString("dd MMM yyyy");
                }
                
            }
            if (nDate == (int)EnumCompareOperator.Between)
            {
                if (String.IsNullOrEmpty(sHeader))
                {
                    sHeader = "Approve Date " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndTime.ToString("dd MMM yyyy");
                }
                else
                {
                    sHeader = " And Approve Date " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndTime.ToString("dd MMM yyyy");
                }
                
            }
            #endregion

            if (string.IsNullOrEmpty(sValue))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                string sSQL = GetSQL(sValue);
                _oFabricFSStatuss = FabricFSStatus.GetsFabricFSStatus(sSQL, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            if (_oFabricFSStatuss.Count > 0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptFabricFSStatusPreview oReport = new rptFabricFSStatusPreview();
                byte[] abytes = oReport.PrepareReport(_oFabricFSStatuss, oCompany, nReportType, sHeader);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            }
        }
        public void FabricFSStatusPreviewExcel(string sValue, double nts)
        {
            _oFabricFSStatuss = new List<FabricFSStatus>();
            _oFabricFSStatus = new FabricFSStatus();
            int nReportType = Convert.ToInt32(sValue.Split('~')[17]);
            string sHeader = "";
            int nDate = 0;
            DateTime sStartDate = DateTime.MinValue;
            DateTime sEndTime = DateTime.MinValue;

            #region SET PO date RANGE
            nDate = Convert.ToInt32(sValue.Split('~')[5]);
            sStartDate = Convert.ToDateTime(sValue.Split('~')[6]);
            sEndTime = Convert.ToDateTime(sValue.Split('~')[7]);
            if (nDate == (int)EnumCompareOperator.EqualTo)
            {
                sHeader = "PO Date " + sStartDate.ToString("dd MMM yyyy");
            }
            if (nDate == (int)EnumCompareOperator.Between)
            {
                sHeader = "PO Date " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndTime.ToString("dd MMM yyyy");
            }
            #endregion
            #region SET Approve date RANGE
            nDate = Convert.ToInt32(sValue.Split('~')[8]);
            sStartDate = Convert.ToDateTime(sValue.Split('~')[9]);
            sEndTime = Convert.ToDateTime(sValue.Split('~')[10]);
            if (nDate == (int)EnumCompareOperator.EqualTo)
            {
                if (String.IsNullOrEmpty(sHeader))
                {
                    sHeader = "Approve Date " + sStartDate.ToString("dd MMM yyyy");
                }
                else
                {
                    sHeader = " AND Approve Date " + sStartDate.ToString("dd MMM yyyy");
                }

            }
            if (nDate == (int)EnumCompareOperator.Between)
            {
                if (String.IsNullOrEmpty(sHeader))
                {
                    sHeader = "Approve Date " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndTime.ToString("dd MMM yyyy");
                }
                else
                {
                    sHeader = " And Approve Date " + sStartDate.ToString("dd MMM yyyy") + " To " + sEndTime.ToString("dd MMM yyyy");
                }
            }
            #endregion

            if (string.IsNullOrEmpty(sValue))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                string sSQL = GetSQL(sValue);
                _oFabricFSStatuss = FabricFSStatus.GetsFabricFSStatus(sSQL, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

             try
            {
                 Company oCompany = new Company();               
                 oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                 if (_oFabricFSStatuss == null)
                {
                    throw new Exception("Sorry, No Data Found!");
                }     
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    int colIndex = 2;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Status Report");
                        sheet.Name = "Fabric Status Report";
                        sheet.Column(colIndex++).Width = 10; //SL
                        sheet.Column(colIndex++).Width = 20; //PO No
                        sheet.Column(colIndex++).Width = 20; //PO Date
                        sheet.Column(colIndex++).Width = 20; //Approve Date
                        sheet.Column(colIndex++).Width = 20; //Approve Delivery Date
                        sheet.Column(colIndex++).Width = 20; //Last Delivery Date
                        sheet.Column(colIndex++).Width = 20; //Lead Time
                        sheet.Column(colIndex++).Width = 30; //Buyer
                        sheet.Column(colIndex++).Width = 35; //No Of Dispo
                        sheet.Column(colIndex++).Width = 20; //Order Qty
                        sheet.Column(colIndex++).Width = 20; //Delivery Qty
                        sheet.Column(colIndex++).Width = 20; //Remarks

                        int nTotalColumn = 12;
                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 12].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 12].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Fabric Status Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex++;

                        sheet.Cells[rowIndex, 2, rowIndex, 12].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = sHeader; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Report Body
                        int nSL = 1;
                        colIndex = 2;
                        var FabricFSStatusList = _oFabricFSStatuss.GroupBy(x => new { x.MktPersonName })
                                              .OrderBy(x => x.Key.MktPersonName)
                                              .Select(x => new
                                              {
                                                  MarketingPersonName = x.Key.MktPersonName,
                                                  _FabricFSStatusList = x.OrderBy(c => c.SCNo),
                                                  TotalQty = x.Sum(y => y.Qty),
                                                  TotalDCQty = x.Sum(y => y.QtyDC)
                                              });
                        foreach(var oData in FabricFSStatusList){
                            nSL = 1;
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = oData.MarketingPersonName; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            rowIndex++;
                            #region Header 1
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "PO No"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "PO Date"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Approve Date"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "App. Delivery Date"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Last Delivery Date"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Lead Time"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "No Of Dispo"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Delivery Qty"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                             foreach (var oItem in oData._FabricFSStatusList)
                            {
                                colIndex = 2;
                               
                                cell = sheet.Cells[rowIndex,colIndex++]; cell.Value = Convert.ToInt16(nSL); cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SCNo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SCDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ApproveDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AppDCDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LastDCDateInString; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TimeSpan; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NoofDispo; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.QtyDC; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RemarksST; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;                           
                                nSL++;
                                rowIndex++;
                            }

                             cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Total:"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                             cell = sheet.Cells[rowIndex, 11]; cell.Value = oData.TotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                             cell = sheet.Cells[rowIndex, 12]; cell.Value = oData.TotalDCQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.WrapText = true; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.WrapText = true; cell.Style.Font.Bold = false; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                             border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;                             
                             rowIndex++;
                        
                        }
                         #endregion
                       
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=RPT_FabricStatusReport.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
            }
           


                catch (Exception ex)
                {
                    _oFabricFSStatuss = new List<FabricFSStatus>();
                    _oFabricFSStatus = new FabricFSStatus();
                    _oFabricFSStatus.ErrorMessage = ex.Message;
                    _oFabricFSStatuss.Add(_oFabricFSStatus);
                }
            
        }
        public ActionResult Print_HWStatement(int nID, int nBUID)
        {
            List<DUHardWinding> _oDUHardWindings = new List<DUHardWinding>();
            List<DUHardWinding> oDUHardWindingList = new List<DUHardWinding>();

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            string sErrorMessage = "";
            try
            {
                if (nID <= 0)
                {
                    throw new Exception("Nothing To Print.");
                }
                else
                {
                    string sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID IN (SELECT DD.DyeingOrderID FROM DyeingOrderFabricDetail as DD where DD.FSCDetailID= " + nID + ")";
                    oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    oFabricExecutionOrderSpecification = FabricExecutionOrderSpecification.Get(nID, (int)Session[SessionInfo.currentUserID]);


                    if (oDyeingOrderDetails.Count <= 0)
                    {
                        throw new Exception("No Data Found.");
                    }
                    _oDUHardWindings = DUHardWinding.Gets("SELECT * FROM View_DUHardWinding WHERE DyeingOrderDetailID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    oRouteSheetPackings = RouteSheetPacking.Gets("Select * from view_RouteSheetPacking where PackedByEmpID<>0 and DyeingOrderDetailID in (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);

                    oDyeingOrders = DyeingOrder.Gets("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT * FROM View_DyeingOrderFabricDetail  as DD   where DD.FSCDetailID= " + nID + "", (int)Session[SessionInfo.currentUserID]);
                    if (oDyeingOrderFabricDetails.Count > 0)
                    {
                        sSQL = "Select * from View_FabricExecutionOrderYarnReceive where FSCDID>0 and FEOSID IN (" + string.Join(",", oDyeingOrderFabricDetails.Select(x => x.FEOSID)) + ")";
                        oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    }
                    if (oFabricExecutionOrderYarnReceives.Count > 0)
                    {
                        oDyeingOrderFabricDetails.ForEach(x =>
                        {
                            x.Qty_Assign = _oDUHardWindings.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Select(o => o.Qty).Sum();
                            x.Qty_RS = oFabricExecutionOrderYarnReceives.Where(p => p.FEOSDID == x.FEOSDID).Select(o => o.ReceiveQty).Sum();
                            x.Length = oFabricExecutionOrderYarnReceives.Where(p => p.FEOSDID == x.FEOSDID).Select(o => o.TFLength).Sum();

                        });
                        _oDUHardWindings.ForEach(x =>
                        {
                            x.Qty_RSOut = oFabricExecutionOrderYarnReceives.Where(p => p.IssueLotID == x.LotID).Select(o => o.ReceiveQty).Sum();
                        });
                    }
                }

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception e)
            {
                sErrorMessage = e.Message;
            }
            if (!string.IsNullOrEmpty(sErrorMessage))
            {
                rptErrorMessage orptErrorMessage = new rptErrorMessage();
                byte[] abytes_error = orptErrorMessage.PrepareReport(sErrorMessage);
                return File(abytes_error, "application/pdf");
            }

            rptHWStatement oReport = new rptHWStatement();
            byte[] abytes = oReport.PrepareReport(_oDUHardWindings, oDyeingOrders, oDyeingOrderDetails, oDyeingOrderFabricDetails, oBusinessUnit, oCompany, oFabricExecutionOrderYarnReceives, oRouteSheetPackings, oFabricExecutionOrderSpecification);
            return File(abytes, "application/pdf");
        }
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
        public ActionResult PrintStatementForDeliveryLog(int nFSCID, double nts, int nBUID)
        {
            FabricSalesContract oFabricSalesContract = new FabricSalesContract();
            List<FabricExpDelivery> oFabricExpDeliverys = new List<FabricExpDelivery>();
            if (nFSCID > 0)
            {
                oFabricSalesContract = oFabricSalesContract.Get(nFSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oFabricSalesContract.FabricSalesContractDetails = FabricSalesContractDetail.Gets(nFSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricExpDeliverys = FabricExpDelivery.Gets("SELECT * FROM View_FabricExpDelivery WHERE FSCDID IN( SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricSalesContractID = " + nFSCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (oFabricExpDeliverys.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nBUID > 0)
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                rptPrintStatementForDeliveryLog oReport = new rptPrintStatementForDeliveryLog();
                byte[] abytes = oReport.PrepareReport(oFabricSalesContract, oFabricExpDeliverys, oCompany);
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