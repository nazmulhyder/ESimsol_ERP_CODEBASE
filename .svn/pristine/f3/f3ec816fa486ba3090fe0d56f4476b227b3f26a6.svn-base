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
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;

namespace ESimSolFinancial.Controllers
{
    public class GRNRegisterController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<GRNRegister> _oGRNRegisters = new List<GRNRegister>();        

        #region Actions
        public ActionResult ViewGRNRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            GRNRegister oGRNRegister = new GRNRegister();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApproveBy FROM GRN AS MM WHERE ISNULL(MM.ApproveBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            ViewBag.GRNType = EnumObject.jGets(typeof(EnumGRNType));
            if (buid > 0)
            {
                oImportProducts = ImportProduct.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oImportProducts = ImportProduct.Gets((int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.ImportProducts = oImportProducts;
            ViewBag.BUID = buid;
            ViewBag.IsRateView = bIsRateView;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.GRNStates = EnumObject.jGets(typeof(EnumGRNStatus));         
            return View(oGRNRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(GRNRegister oGRNRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oGRNRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintGRNRegister(double ts)
        {
            GRNRegister oGRNRegister = new GRNRegister();
            try
            {
                _sErrorMesage = "";
                _oGRNRegisters = new List<GRNRegister>();                
                oGRNRegister = (GRNRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oGRNRegister);
                _oGRNRegisters = GRNRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oGRNRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oGRNRegisters = new List<GRNRegister>();
                _sErrorMesage = ex.Message;
            }

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oGRNRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oGRNRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptGRNRegisters oReport = new rptGRNRegisters();
                byte[] abytes = oReport.PrepareReport(_oGRNRegisters, oCompany, oGRNRegister.ReportLayout, _sDateRange, bIsRateView);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintGRNRegisterTwo(double ts)
        {
            GRNRegister oGRNRegister = new GRNRegister();
            try
            {
                _sErrorMesage = "";
                _oGRNRegisters = new List<GRNRegister>();
                oGRNRegister = (GRNRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oGRNRegister);
                _oGRNRegisters = GRNRegister.GetsTwo(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oGRNRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oGRNRegisters = new List<GRNRegister>();
                _sErrorMesage = ex.Message;
            }

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oGRNRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oGRNRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptGRNRegisters oReport = new rptGRNRegisters();
                byte[] abytes = oReport.PrepareReportTwo(_oGRNRegisters, oCompany, oGRNRegister.ReportLayout, _sDateRange, bIsRateView);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        public void ExportToExcelGRNRegisterTwo(double ts)
        {
            GRNRegister oGRNRegister = new GRNRegister();
            try
            {
                _sErrorMesage = "";
                _oGRNRegisters = new List<GRNRegister>();
                oGRNRegister = (GRNRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oGRNRegister);
                _oGRNRegisters = GRNRegister.GetsTwo(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oGRNRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oGRNRegisters = new List<GRNRegister>();
                _sErrorMesage = ex.Message;
            }

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oGRNRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oGRNRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
                if (_oGRNRegisters.Count > 0)
                {
                    oGRNRegister.ProductType = _oGRNRegisters[0].ProductType;
                    oGRNRegister.ProductTypeSt = _oGRNRegisters[0].ProductTypeSt;
                }
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Name of Supplier", Width = 20f, IsRotate = false });
               if( oGRNRegister.ProductType==EnumProductNature.Yarn)
               {
                table_header.Add(new TableHeader { Header = "Yarn Count", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yarn Type", Width = 20f, IsRotate = false });
               }
               else
               {
                table_header.Add(new TableHeader { Header = oGRNRegister.ProductType.ToString(), Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = oGRNRegister.ProductTypeSt, Width = 20f, IsRotate = false });
               }
                table_header.Add(new TableHeader { Header = "Lot", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Unit", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/C No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Received Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Gate Entry No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN/MRIR No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Rate(USD)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount (USD)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Rate(BDT)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amount (BDT)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Remark", Width = 15f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("GRN Register(Details)");
                    sheet.Name = "GRN Register(Details)";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "GRN Register(Details)"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;
                    #endregion

                    nStartCol = 2;
                    #region Header Title
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion


                    #region Data
                    int nCount = 0;
                    foreach (var oItem1 in _oGRNRegisters)
                    {
                        nStartCol = 2;
                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.GRNDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.SupplierName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.GroupName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.ProductName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.LotNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.PINo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        _sFormatter = " #,##0.00;(#,##0.00)";
                        FillCell(sheet, nRowIndex, nStartCol++, oItem1.PIQty.ToString(), true);
                        FillCellMerge(ref sheet, oItem1.MUSymbol, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.LCNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.ChallanNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem1.RefQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem1.ReceivedQty.ToString(), true);
                        FillCellMerge(ref sheet, oItem1.GatePassNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem1.GRNNo + "" + oItem1.MRIRNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem1.UnitPrice.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, (oItem1.UnitPrice * oItem1.ReceivedQty).ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, (oItem1.UnitPrice * oItem1.CRate).ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, (oItem1.UnitPrice * oItem1.ReceivedQty * oItem1.CRate).ToString(), true);
                        FillCellMerge(ref sheet, oItem1.Remarks, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    nStartCol = 2;
                    FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 10, true, ExcelHorizontalAlignment.Right);
                    _sFormatter = " #,##0.00;(#,##0.00)"; nStartCol++;
                    FillCell(sheet, nRowIndex, nStartCol++, _oGRNRegisters.Sum(x=>x.RefQty).ToString(), true);
                    FillCell(sheet, nRowIndex, nStartCol++, _oGRNRegisters.Sum(x => x.ReceivedQty).ToString(), true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    FillCell(sheet, nRowIndex, nStartCol++, _oGRNRegisters.Sum(x => (x.ReceivedQty * x.UnitPrice)).ToString(), true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    FillCell(sheet, nRowIndex, nStartCol++, _oGRNRegisters.Sum(x => (x.ReceivedQty * x.UnitPrice*x.CRate)).ToString(), true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex, nStartCol, nStartCol++);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=GRN_Register_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion


            }
        }

        [HttpPost]
        public JsonResult GetsPurchaseInvocie(GRN oGRN)
        {
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseInvoice AS TT WHERE  ISNULL(TT.ApprovedBy,0)!=0 AND TT.BUID= " + oGRN.BUID.ToString();
                oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseInvoices = new List<PurchaseInvoice>();
                PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = ex.Message;
                oPurchaseInvoices.Add(oPurchaseInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPurchaseOrders(GRN oGRN)
        {
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseOrder AS TT WHERE  ISNULL(TT.ApproveBy,0)!=0 AND TT.BUID= " + oGRN.BUID.ToString();
                oPurchaseOrders = PurchaseOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseOrders = new List<PurchaseOrder>();
                PurchaseOrder oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = ex.Message;
                oPurchaseOrders.Add(oPurchaseOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsWorkOrder(GRN oGRN)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrder AS TT WHERE ISNULL(TT.ApproveBy,0)!=0 AND  TT.BUID = " + oGRN.BUID.ToString();
                oWorkOrders = WorkOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWorkOrders = new List<WorkOrder>();
                WorkOrder oWorkOrder = new WorkOrder();
                oWorkOrder.ErrorMessage = ex.Message;
                oWorkOrders.Add(oWorkOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportPIs(GRN oGRN)
        {
            List<ImportPI> oImportPIs = new List<ImportPI>();
            try
            {
                string sSQL = "SELECT top(150)* FROM View_ImportPI AS TT WHERE ISNULL(TT.ApprovedBy,0)!=0 AND  TT.BUID = " + oGRN.BUID.ToString();
                if(!string.IsNullOrEmpty(oGRN.RefObjectNo))
                {
                    oGRN.RefObjectNo = oGRN.RefObjectNo.Trim();
                    sSQL = sSQL + " and ImportPINo like '%" + oGRN.RefObjectNo+ "%'";
                }
                sSQL = sSQL + " Order by ImportPIID DESC";
                oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPIs = new List<ImportPI>();
                ImportPI oImportPI = new ImportPI();
                oImportPI.ErrorMessage = ex.Message;
                oImportPIs.Add(oImportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsImportInvocie(GRN oGRN)
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            try
            {
                string sSQL = "SELECT top(150)* FROM View_ImportInvoice AS TT WHERE   TT.BUID = " + oGRN.BUID.ToString();
                if (!string.IsNullOrEmpty(oGRN.RefObjectNo))
                {
                    sSQL = sSQL + " and ImportInvoiceNo like '%" + oGRN.RefObjectNo + "%'";
                }
                sSQL = sSQL + " Order by ImportInvoiceID DESC";
                oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            }
            catch (Exception ex)
            {
                oImportInvoices = new List<ImportInvoice>();
                ImportInvoice oImportInvoice = new ImportInvoice();
                oImportInvoice.ErrorMessage = ex.Message;
                oImportInvoices.Add(oImportInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsConsumptions(GRN oGRN)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            try
            {
                if (oGRN.RefObjectNo == null) { oGRN.RefObjectNo = ""; }
                string sSQL = "SELECT * FROM View_ConsumptionRequisition AS TT WHERE TT.CRType=2 AND ISNULL(TT.ApprovedBy,0)!=0 AND TT.RequisitionForName LIKE '%" + oGRN.RefObjectNo + "%' AND TT.BUID = " + oGRN.BUID.ToString();
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
                ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = ex.Message;
                oConsumptionRequisitions.Add(oConsumptionRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }                

        #region Excel
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber)
                cell.Value = Convert.ToDouble(sVal);
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        public void ExportToExcelGRNRegister(double ts)
        {
            int nGRNID = -999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            GRNRegister oGRNRegister = new GRNRegister();
            try
            {
                _sErrorMesage = "";
                _oGRNRegisters = new List<GRNRegister>();
                oGRNRegister = (GRNRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oGRNRegister);
                _oGRNRegisters = GRNRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oGRNRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oGRNRegisters = new List<GRNRegister>();
                _sErrorMesage = ex.Message;
            }

            bool _bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                _bIsRateView = true;
            }

            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            if (oGRNRegister.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oGRNRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GL Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN Status", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Style", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Ref. No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Store Name", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Supplier Name", Width = 45f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Brand Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Approved By", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN Date", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M. Unit", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Ref Qty", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "GRN Qty", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Extra Qty", Width = 25f, IsRotate = false });


                if (_bIsRateView)
                {
                    table_header.Add(new TableHeader { Header = "Rate", Width = 25f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Amount", Width = 25f, IsRotate = false });
                }
                #endregion

                #region Layout Wise Header
                if (oGRNRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    Header = "Product Wise"; HeaderColumn = "Product Name : ";
                }
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    #region Detail Data
                    var sheet = excelPackage.Workbook.Worksheets.Add("GRN Register");
                    sheet.Name = "GRN Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "GRN Register (" + Header + ") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Group By Layout Wise
                    var data = _oGRNRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                    {
                        HeaderName = key.ProductName,
                        TotalQty = grp.Sum(x => x.ReceivedQty),
                        TotalAmount = grp.Sum(x => x.Amount),
                        Results = grp.ToList()
                    });

                    //if (oGRNRegister.ReportLayout == EnumReportLayout.ProductWise)
                    //{
                    //    data = _oGRNRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                    //    {
                    //        HeaderName = key.ProductName,
                    //        TotalQty = grp.Sum(x => x.Qty),
                    //        TotalAmount = grp.Sum(x => x.Amount),
                    //        Results = grp.ToList()
                    //    });
                    //}
                    //else if (oGRNRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                    //{
                    //    data = _oGRNRegisters.GroupBy(x => new { x.DateofInvoiceSt }, (key, grp) => new
                    //    {
                    //        HeaderName = key.DateofInvoiceSt,
                    //        TotalQty = grp.Sum(x => x.Qty),
                    //        TotalAmount = grp.Sum(x => x.Amount),
                    //        Results = grp.ToList()
                    //    });
                    //}
                    #endregion

                    string sCurrencySymbol = "";
                    #region Data
                    foreach (var oItem in data)
                    {
                        nRowIndex++;

                        nStartCol = 2;
                        FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        nRowIndex++;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        nGRNID = 0;
                        nRowIndex++; int nCount = 0, nRowSpan = 0;
                        foreach (var obj in oItem.Results)
                        {
                            #region Product Wise Merge
                            if (nGRNID != obj.GRNID)
                            {
                                if (nCount > 0)
                                {
                                    nStartCol = 16;
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.ReceivedQty).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                                    if (_bIsRateView)
                                    {
                                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);//Rate
                                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.Amount).ToString(), true, true);//amount
                                    }
                                    nRowIndex++;
                                }

                                nStartCol = 2;
                                nRowSpan = oItem.Results.Where(GRNR => GRNR.GRNID == obj.GRNID && GRNR.ProductID == obj.ProductID).ToList().Count;

                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.GRNNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.GLDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.GRNStatusSt.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.StyleNo.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.RefObjectNo.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.ChallanNo.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.ColorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.StoreName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.SupplierName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.BrandName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.BuyerName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            }
                            #endregion

                            nStartCol = 14;

                            FillCell(sheet, nRowIndex, nStartCol++, obj.ApprovedByName, false);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo, false);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.GRNDateSt, false);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.MUSymbol, false);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.RefQty.ToString(), true);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.ReceivedQty.ToString(), true);
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.ExtraQty < 0 ? "0" : obj.ExtraQty.ToString(), true);

                            if (_bIsRateView)
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString(), true);
                                FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true);
                            }
                            nRowIndex++;

                            nGRNID = obj.GRNID;
                            sCurrencySymbol = obj.CurrencySymbol;
                        }

                        nStartCol = 16;
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.ReceivedQty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        if (_bIsRateView)
                        {
                            FillCell(sheet, nRowIndex, ++nStartCol, "", false);//Rate
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.Amount).ToString(), true, true);//Amount
                        }
                        nRowIndex++;

                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 16, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
                        if (_bIsRateView)
                        {
                            FillCell(sheet, nRowIndex, ++nStartCol, "", false, true); //rate 
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalAmount.ToString(), true, true); //amount
                        }
                        nRowIndex++;
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 16, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    if (_bIsRateView)
                    {
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);//Rate
                        FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalAmount).ToString(), true, true); //Amount
                    }
                    nRowIndex++;
                    #endregion
                    #endregion
                    #endregion
                    #region Summary
                    var Summarysheet = excelPackage.Workbook.Worksheets.Add("Update Summary");
                    Summarysheet.Name = "Update Summary";

                   int  colIndex = 1;
                    Summarysheet.Column(++colIndex).Width = 8; //SL
                    Summarysheet.Column(++colIndex).Width = 30; //Product Name
                    Summarysheet.Column(++colIndex).Width = 25; //Supplier Name
                    Summarysheet.Column(++colIndex).Width = 15; //M Unit
                    Summarysheet.Column(++colIndex).Width = 15; //Grn Qty
                    Summarysheet.Column(++colIndex).Width = 13; //Rate(BDT)
                    Summarysheet.Column(++colIndex).Width = 13; //Amount(BDT)
                    colIndex = 1;
                    int rowIndex = 2;

                    #region Address & Date
                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    colIndex = 1;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Supplier Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "GRN Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Rate("+oCompany.CurrencyName+")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount(" + oCompany.CurrencyName + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    var dataSummary = _oGRNRegisters.GroupBy(x => new { x.ProductName, x.SupplierName }, (key, grp) => new
                    {
                        ProductName = key.ProductName,
                        SupplierName = key.SupplierName,
                        TotalQty = grp.Sum(x => x.ReceivedQty),
                        TotalAmount = grp.Sum(x => x.Amount),
                        UnitName=grp.ToList().Select(x=>x.MUName).FirstOrDefault(),
                        Results = grp.ToList()
                    });

                    dataSummary = dataSummary.OrderBy(x => x.ProductName);
                    rowIndex++;
                    colIndex = 1;
                     int Count = 0;
                    int nStartRow = rowIndex;

                    foreach (var oItem in dataSummary)
                    {
                        colIndex = 1;
                        Count++;
                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = Count; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.TotalQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.TotalAmount / oItem.TotalQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }


                    cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    string sStartCell = Global.GetExcelCellName(nStartRow, 8);
                   string  sEndCell = Global.GetExcelCellName(rowIndex - 1, 8);

                   cell = Summarysheet.Cells[rowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold =true; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = Summarysheet.Cells[1, 1, rowIndex, 10];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion
                    cell = sheet.Cells[1, 1, nRowIndex, 17];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);


                   

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=GRNRegister(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        #endregion

        #endregion

        #region Support Functions
        private string GetSQL(GRNRegister oGRNRegister)
        {
            _sDateRange = "";
            string sSearchingData = oGRNRegister.SearchingData;
            EnumCompareOperator eGRNDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dGRNStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dGRNEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

            EnumCompareOperator eGLDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dGLDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dGLDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

            EnumCompareOperator eApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

            EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
            double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

            int nRefType = Convert.ToInt32(sSearchingData.Split('~')[12]);
            string sRefNo = Convert.ToString(sSearchingData.Split('~')[13]);


            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oGRNRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oGRNRegister.BUID.ToString();
            }
            #endregion

            #region GRNNo
            if (oGRNRegister.GRNNo != null && oGRNRegister.GRNNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " GRNNo LIKE'%" + oGRNRegister.GRNNo + "%'";
            }
            #endregion

            #region ChallanNo
            if (oGRNRegister.ChallanNo != null && oGRNRegister.ChallanNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ChallanNo LIKE'%" + oGRNRegister.ChallanNo + "%'";
            }
            #endregion

            #region LotNo
            if (oGRNRegister.LotNo != null && oGRNRegister.LotNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " LotNo LIKE'%" + oGRNRegister.LotNo + "%'";
            }
            #endregion

            #region ColorName
            if (oGRNRegister.ColorName != null && oGRNRegister.ColorName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ColorName LIKE'%" + oGRNRegister.ColorName + "%'";
            }
            #endregion

            #region ApprovedBy
            if (oGRNRegister.ApproveBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ApproveBy =" + oGRNRegister.ApproveBy.ToString();
            }
            #endregion

            #region GRNStatus
            if (oGRNRegister.GRNStatus != EnumGRNStatus.Initialize)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " GRNStatus =" + ((int)oGRNRegister.GRNStatus).ToString();
            }
            #endregion

            #region Remarks
            if (oGRNRegister.Remarks != null && oGRNRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " GRN.Remarks LIKE'%" + oGRNRegister.Remarks + "%'";
            }
            #endregion

            #region Supplier
            if (oGRNRegister.SupplierName != null && oGRNRegister.SupplierName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID IN(" + oGRNRegister.SupplierName + ")";
            }
            #endregion

            #region Style
            if (oGRNRegister.StyleNo != null && oGRNRegister.StyleNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StyleID IN(" + oGRNRegister.StyleNo + ")";
            }
            #endregion

            #region ProductType
            if (oGRNRegister.ProductType != EnumProductNature.Dyeing)
            {
                Global.TagSQL(ref sWhereCluse);
                //sWhereCluse = sWhereCluse + "  RefType =  "+(int)EnumGRNType.ImportPI+" AND  RefObjectID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE  ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE  ProductType =" + ((int)oGRNRegister.ProductType).ToString() + "))";
                sWhereCluse = sWhereCluse + " ProductID in ( SELECT ProductID FROM Product AS HH WHERE HH.Activity = 1 and  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + oGRNRegister.BUID + ", " +(int)oGRNRegister.ProductType+ "))) ";
            }
            #endregion

            #region Store 
            if (oGRNRegister.StoreName != null && oGRNRegister.StoreName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StoreID IN(" + oGRNRegister.StoreName + ")";
            }
            #endregion

            #region Product
            if (oGRNRegister.ProductName != null && oGRNRegister.ProductName != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID IN(" + oGRNRegister.ProductName + ")";
            }
            #endregion

            #region GRN Date
            if (eGRNDate != EnumCompareOperator.None)
            {
                if (eGRNDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "GRN Date @ " + dGRNStartDate.ToString("dd MMM yyyy");
                }
                else if (eGRNDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "GRN Date Not Equal @ " + dGRNStartDate.ToString("dd MMM yyyy");
                }
                else if (eGRNDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "GRN Date Greater Then @ " + dGRNStartDate.ToString("dd MMM yyyy");
                }
                else if (eGRNDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "GRN Date Smaller Then @ " + dGRNStartDate.ToString("dd MMM yyyy");
                }
                else if (eGRNDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "GRN Date Between " + dGRNStartDate.ToString("dd MMM yyyy") + " To " + dGRNEndDate.ToString("dd MMM yyyy");
                }
                else if (eGRNDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "GRN Date NOT Between " + dGRNStartDate.ToString("dd MMM yyyy") + " To " + dGRNEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region GLDate Date
            if (eGLDate != EnumCompareOperator.None)
            {
                if (eGLDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGLDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGLDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGLDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGLDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGLDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approved Date
            if (eApprovedDate != EnumCompareOperator.None)
            {
                if (eApprovedDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eApprovedDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Amount
            if (ePIAmount != EnumCompareOperator.None)
            {
                if (ePIAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount = " + nPIAmountStsrt.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount != " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount > " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount < " + nPIAmountStsrt.ToString("0.00"); ;
                }
                else if (ePIAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
                else if (ePIAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " Amount NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
                }
            }
            #endregion

            #region RefType
            if (nRefType != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RefType = " + nRefType ;
            }
            #endregion

            #region RefNo
            if (sRefNo != null && sRefNo != "")
            {
                if (nRefType != 0)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " GRN.RefObjectID IN (" + sRefNo + ")";
                }
            }
            #endregion

            #region Report Layout
           if (oGRNRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "";
                sOrderBy = " ORDER BY  GRNDate, GRNID, GRNDetailID ASC";
            }
            
            else if (oGRNRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "";
                sOrderBy = " ORDER BY  ContractorID, GRNID, GRNDetailID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "";
                sOrderBy = " ORDER BY ProductID, GRNID, GRNDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
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
        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}

