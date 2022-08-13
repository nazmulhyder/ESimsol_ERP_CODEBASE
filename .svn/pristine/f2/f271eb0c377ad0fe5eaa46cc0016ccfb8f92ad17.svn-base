using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class FSCFollowUpController : Controller
    {
        #region Declaration

        FSCFollowUp _oFSCFollowUp = new FSCFollowUp();
        List<FSCFollowUp> _oFSCFollowUps = new List<FSCFollowUp>();
        #endregion

        #region Actions
        public ActionResult ViewFSCFollowUp(int nSampleOrBulk, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FSCFollowUp).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFSCFollowUps = new List<FSCFollowUp>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.POStatus = EnumObject.jGets(typeof(EnumPOState)).Where(x => x.id == (int)EnumPOState.Running || x.id == (int)EnumPOState.Closed);
            ViewBag.BUID = buid;
            ViewBag.nSampleOrBulk = nSampleOrBulk;
            return View(_oFSCFollowUps);
        }
        #endregion

        #region Adv Search
        [HttpPost]
        public JsonResult AdvSearch(FSCFollowUp oFSCFollowUp)
        {
            _oFSCFollowUps = new List<FSCFollowUp>();
            try
            {
                string sSQL = MakeSQL(oFSCFollowUp);
                _oFSCFollowUps = FSCFollowUp.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFSCFollowUps = new List<FSCFollowUp>();
                oFSCFollowUp = new FSCFollowUp();
                oFSCFollowUp.ErrorMessage = ex.Message;
                _oFSCFollowUps.Add(oFSCFollowUp);
            }
            var jsonResult = Json(_oFSCFollowUps, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FSCFollowUp oFSCFollowUp)
        {
            int nPODateAdv = 0, nSampleOrBulk = 0;
            DateTime dFromPODateAdv = DateTime.Today, dToPODateAdv = DateTime.Today;
            string sPONoAdv = "", sDispoNoAdv = "", sFabricNoAdv = "";
            string sReturn = " WHERE ISNULL(FSCD.[Status],0) NOT IN (6,7) ";
            if (!string.IsNullOrEmpty(oFSCFollowUp.ErrorMessage))
            {
                sPONoAdv = Convert.ToString(oFSCFollowUp.ErrorMessage.Split('~')[0]);
                sDispoNoAdv = Convert.ToString(oFSCFollowUp.ErrorMessage.Split('~')[1]);
                sFabricNoAdv = Convert.ToString(oFSCFollowUp.ErrorMessage.Split('~')[2]);
                nPODateAdv = Convert.ToInt32(oFSCFollowUp.ErrorMessage.Split('~')[3]);
                dFromPODateAdv = Convert.ToDateTime(oFSCFollowUp.ErrorMessage.Split('~')[4]);
                dToPODateAdv = Convert.ToDateTime(oFSCFollowUp.ErrorMessage.Split('~')[5]);
                nSampleOrBulk = Convert.ToInt32(oFSCFollowUp.ErrorMessage.Split('~')[6]);
            }

            if (nSampleOrBulk == 1)//for sample
                sReturn += " AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.OrderType IN (2,9)) ";
            else if (nSampleOrBulk == 2)//for bulk
                sReturn += " AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.OrderType IN (3)) ";
            else//for sample and bulk
                sReturn += " AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.OrderType IN (2,9,3)) ";


            #region PO No
            if (!string.IsNullOrEmpty(sPONoAdv))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.SCNo LIKE '%" + sPONoAdv + "%') ";
            }
            #endregion

            #region Dispo No
            if (!string.IsNullOrEmpty(sDispoNoAdv))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FSCD.ExeNo LIKE '%" + sDispoNoAdv + "%' ";
            }
            #endregion

            #region Fabric No
            if (!string.IsNullOrEmpty(sFabricNoAdv))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FSCD.FabricID IN (SELECT Fabric.FabricID FROM Fabric WHERE Fabric.FabricNo LIKE '%" + sFabricNoAdv + "%') ";
            }
            #endregion

            #region PO Date
            if (nPODateAdv != (int)EnumCompareOperator.None)
            {
                if (nPODateAdv == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFromPODateAdv.ToString("dd MMM yyyy") + "', 106)) )";
                }
                else if (nPODateAdv == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFromPODateAdv.ToString("dd MMM yyyy") + "', 106)) )";
                }
                else if (nPODateAdv == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFromPODateAdv.ToString("dd MMM yyyy") + "', 106)) )";
                }
                else if (nPODateAdv == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFromPODateAdv.ToString("dd MMM yyyy") + "', 106)) )";
                }
                else if (nPODateAdv == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFromPODateAdv.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dToPODateAdv.ToString("dd MMM yyyy") + "', 106)) )";
                }
                else if (nPODateAdv == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFromPODateAdv.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dToPODateAdv.ToString("dd MMM yyyy") + "', 106)) )";
                }
            }
            #endregion

            string sSQL = sReturn;
            return sSQL;
        }
        #endregion

        #region Functions
        [HttpGet]
        public JsonResult GetData(int nSampleOrBulk)
        {
            _oFSCFollowUps = new List<FSCFollowUp>();
            try
            {
                string sSQL = " WHERE ISNULL(FSCD.[Status],0) NOT IN (6,7) AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),FSC.SCDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), DATEADD(Day,-180,GETDATE()), 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), GETDATE(), 106)) )";
                if (nSampleOrBulk == 1)//for sample
                    sSQL += " AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.OrderType IN (2,9)) ";
                else if (nSampleOrBulk == 2)//for bulk
                    sSQL += " AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.OrderType IN (3)) ";
                else//for sample and bulk
                    sSQL += " AND FSCD.FabricSalesContractID IN (SELECT FSC.FabricSalesContractID FROM FabricSalesContract AS FSC WHERE FSC.OrderType IN (2,9,3)) ";
                _oFSCFollowUps = FSCFollowUp.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFSCFollowUps = new List<FSCFollowUp>();
                FSCFollowUp oFSCFollowUp = new FSCFollowUp();
                oFSCFollowUp.ErrorMessage = ex.Message;
                _oFSCFollowUps.Add(oFSCFollowUp);
            }
            var jsonResult = Json(_oFSCFollowUps, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetHistory(FSCFollowUp oFSCFollowUp)
        {
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            List<FabricSCHistory> oFabricSCHistorys = new List<FabricSCHistory>();
            try
            {
                
                oFSCD = oFSCD.Get(oFSCFollowUp.FSCDetailID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_FabricSCHistory WHERE FabricSCDetailID=" + oFSCFollowUp.FSCDetailID + " AND OrderType IN (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.BulkOrder+ "," + (int)EnumOrderType.ReConing + ") ORDER BY DBServerDateTime DESC";
                oFSCD.FabricSCHistorys = FabricSCHistory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFSCD = new FabricSalesContractDetail();
                oFSCD.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(oFSCD, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult UpdateStatus(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            FabricSalesContractDetail _oFabricSalesContractDetail = new FabricSalesContractDetail();
            try
            {
                _oFabricSalesContractDetail = oFabricSalesContractDetail.UpdateStatus((int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_FabricSCHistory WHERE FabricSCDetailID=" + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND OrderType IN (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.ReConing + ") ORDER BY DBServerDateTime DESC";
                _oFabricSalesContractDetail.FabricSCHistorys = FabricSCHistory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricSalesContractDetail = new FabricSalesContractDetail();
                _oFabricSalesContractDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Excel
        public void ExportXL(string sParams)
        {
            _oFSCFollowUps = new List<FSCFollowUp>();
            FSCFollowUp oFSCFollowUp = new FSCFollowUp();
            oFSCFollowUp.ErrorMessage = sParams;
            string sSQL = MakeSQL(oFSCFollowUp);
            _oFSCFollowUps = FSCFollowUp.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oFSCFollowUps.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo Qty(Yds)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "End Buyer", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Acc. Holder", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Y/D Production Schedule Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req Dyed Yarn Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Y/D Production Schedule Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Lot Assign Date(End)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Lot Assign Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn SRS Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn SRS Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Receive Date(S/W)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Softwinding Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Send to Floor Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Dyeing Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Load Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Batch Unload Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DC Out Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Hydro Load Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dryer UnLoad Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "QC Approval Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approval Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "H/W Recd Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fresh Dyeing Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Beam Transfer Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Received in Warpping", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warping Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warping Exe. Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warping End Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sizing Exe. Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sizing Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Drawing Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Drawing End Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loom Production Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weiving/Loom Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Grey Inspection Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Grey Fabric Recd", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finishing Batch Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finishing Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Pretreatment Start Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Pretreatment Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Solid Dyeing start date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Solid Dyeing Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Final Inpection Receive Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Final Inpection Receive Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Final Inspection Delivery Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Store Recd Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delivery to Party Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Return Qty", Width = 15f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FOLLOW UP REPORT");

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = "FOLLOW UP REPORT"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion

                    #region Data
                    int nSL = 0;
                    foreach (FSCFollowUp oItem in _oFSCFollowUps)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nSL).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.SCDateInString, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ExeDateInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Qty_PO.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Qty_Dispo.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.MKTPersonName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProcessTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateDUPScheduleInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyReqDyed.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyDUPSchedule.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateLotAssignInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyLotAssign.ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateIssueDUReqInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyDUReq.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateReceiveDUReqInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtySoftWinding.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateRSInFloorInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateBatchloadInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyDyeMachine.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateBatchUnloadInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateDCOutInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateHydroLoadInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateDryerUnLoadInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyApproval.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateApprovalInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateHWRecdInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyFreshDye.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateBeamTrInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyBeamTr.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateWarpingStartInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyWarping.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateWarpingEndInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtySizing.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateSizingStartInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateDrawingStartInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateDrawingEndInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyLoom.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateLoomStartInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyGreyIns.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyGreyRecd.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyFnBatch.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateFinishingInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DatePretreatmentInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyPretreatment.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateFnDyeingInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyFnDyeing.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateFNInsRecdInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyFnInspRecd.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DateFNInsDCInString, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyStoreRecd.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyDC.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.QtyRC.ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        nRowIndex++;

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.Qty_PO).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.Qty_Dispo).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyReqDyed).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyDUPSchedule).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyLotAssign).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyDUReq).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtySoftWinding).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyDyeMachine).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyApproval).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyFreshDye).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyBeamTr).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyWarping).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtySizing).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyLoom).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyGreyIns).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyGreyRecd).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyFnBatch).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyPretreatment).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyFnDyeing).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyFnInspRecd).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyStoreRecd).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyDC).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oFSCFollowUps.Sum(c => c.QtyRC).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FOLLOW_UP_REPORT.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Pdf
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
        public ActionResult PrintDispoStatement(int nID, int nBUID, string nts)
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            List<RPT_Dispo> oRPT_DisposReport = new List<RPT_Dispo>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrderFabricDetail WHERE FSCDetailID =" + nID + ")";
            // + " AND ProductID = "+ nPID ;
            oDURequisitionDetails = DURequisitionDetail.Gets(sSQL + " ORDER BY ReqDate, RequisitionNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sTemo = string.Join(",", oDURequisitionDetails.Select(x => x.DestinationLotID).ToList());
            if (string.IsNullOrEmpty(sTemo))
                sTemo = "0";
            sSQL = "Select * from View_RSRawLot where LotID in (" + sTemo + ")  and DyeingOrderID  in (SELECT DyeingOrderID FROM DyeingOrderFabricDetail WHERE FSCDetailID =" + nID + ")";
            oRSRawLots = RSRawLot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "Select * from View_FabricExecutionOrderYarnReceive where WYarnType in (" + (int)EnumWYarnType.Gray + ") and FSCDID>0 and FSCDID IN (" + nID + ")";
            oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);



            sSQL = " WHERE DODF.FSCDetailID = " + nID;
            oRPT_Dispos = RPT_Dispo.Gets_FYStockDispoWise(sSQL, 1, (int)Session[SessionInfo.currentUserID]);
            sSQL = "select * ,(Select SUM(Qty) from DURequisitionDetail as DUR where  ProductID=HH.ProductID and DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=101 AND ISNULL(ReceiveByID,0)<>0) AND  LotID  in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and   DyeingOrderID =HH.DyeingOrderID )as Qty_SRS"
+ " ,(Select SUM(Qty) from DURequisitionDetail as DUR where ProductID=HH.ProductID and   DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102 AND ISNULL(ReceiveByID,0)<>0) AND  DestinationLotID in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and   DyeingOrderID =HH.DyeingOrderID) as Qty_SRM"
+ ",(Select SUM(Qty) from DyeingOrderFabricDetail as DODF where DODF.FEOSID=HH.FEOSID and DODF.ProductID=HH.ProductID ) as Qty_Dispo"
+ ",isnull((Select SUM(Qty) from View_RSRawLot where LotID  in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID)  and DyeingOrderID=HH.DyeingOrderID and RouteSheetID in (Select RouteSheetID from View_RouteSheetDO where ProductID=HH.ProductID and  DyeingOrderID=HH.DyeingOrderID )),0) as Qty_Dye"
+ " from (Select FLA.LotID, DOF.DyeingOrderID,DOF.ProductID, DOF.FEOSID, Lot.LotNo as StyleNo, Product.ProductName,SUM(FLA.Qty)as Req_GreyYarn from DyeingOrderFabricDetail as DOF left join FabricLotAssign as FLA ON FLA.FEOSDID =DOF.FEOSDID left join Lot as Lot ON Lot.LotID =FLA.LotID"
+ " left join Product as Product ON Product.ProductID =FLA.ProductID where DOF.FSCDetailID=" + nID + "  group by FEOSID, FLA.LotID, DOF.DyeingOrderID,DOF.ProductID,Lot.LotNo,Product.ProductName) as HH order by HH.DyeingOrderID";
            oRPT_DisposReport = RPT_Dispo.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            rpt_RPT_Dispo_Stock oReport = new rpt_RPT_Dispo_Stock();
            rptErrorMessage oErrorReport = new rptErrorMessage();
            byte[] abytes = new byte[1];
            if (oRPT_DisposReport.Count > 0)
            {
                var sDispo = "(" + oRPT_Dispos.Select(x => x.ExeNo).FirstOrDefault() + ")";
                abytes = oReport.PrepareReport(" Dispo Wise Store Report " + sDispo, oRPT_Dispos, oDURequisitionDetails, oRSRawLots, oBusinessUnit, oCompany, oRPT_DisposReport, oFabricExecutionOrderYarnReceives);
            }
            else
            {
                abytes = oErrorReport.PrepareReport("No Data");
            }

            return File(abytes, "application/pdf");
        }
        #endregion

    }

}
