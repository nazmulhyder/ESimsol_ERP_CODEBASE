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
    public class WUSubContractPrintController : Controller
    {   
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        List<WUSubContract> _oWUSubContracts = new List<WUSubContract>();        

        #region Actions
        public ActionResult ViewWUSubContractPrints(int buid, int MenuId)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, MenuId);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WUSubContract).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oWUSubContracts = WUSubContract.Gets(buid, (int)Session[SessionInfo.currentUserID]);

            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Management, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ContractEmployees = oEmployees;

            ViewBag.BUID = buid;
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumWUOrderType));
            ViewBag.PaymentModes = EnumObject.jGets(typeof(EnumInvoicePaymentMode));
            ViewBag.Transportations = EnumObject.jGets(typeof(EnumTransportation));
            ViewBag.WarpWefts = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.Currencys = Currency.Gets("SELECT * FROM Currency", (int)Session[SessionInfo.currentUserID]);
            ViewBag.MeasurementUnits = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit", (int)Session[SessionInfo.currentUserID]);

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

            ViewBag.ReportLayouts = oReportLayouts;

            return View(_oWUSubContracts);
        }

        [HttpPost]
        public JsonResult GetsFabricName(FabricProcess oFabricProcess) // Added By Sagor on 24 May 2014 For Enter Event Searching
        {
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            try
            {
                string sType = oFabricProcess.Params.Split('~')[0];
                string sName = oFabricProcess.Params.Split('~')[1].Trim();
                if (sName == "@FabricProcessID") sName = "";
                oFabricProcesss = FabricProcess.GetsByFabricNameType(sName, sType, (int)Session[SessionInfo.currentUserID]);
                if (oFabricProcesss.Count <= 0) { throw new Exception("No information found."); }
            }
            catch (Exception ex)
            {
                oFabricProcess = new FabricProcess();
                oFabricProcess.ErrorMessage = ex.Message;
                oFabricProcesss.Add(oFabricProcess);
            }
            var jsonResult = Json(oFabricProcesss, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.WUSubContract, EnumProductUsages.Yarn, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.WUSubContract, EnumProductUsages.Yarn, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                Product _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetCompositions(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.WUSubContract, EnumProductUsages.Fabric, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.WUSubContract, EnumProductUsages.Fabric, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                Product _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(WUSubContract oWUSubContract)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oWUSubContract);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void PrintExcel(double ts)
        {
            Company oCompany = new Company();
            List<WUSubContract> oWUSubContracts = new List<WUSubContract>();
            WUSubContract oWUSubContract = new WUSubContract();

            try
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract = (WUSubContract)Session[SessionInfo.ParamObj];

                string sSQL = this.GetSQL(oWUSubContract);
                oWUSubContracts = WUSubContract.GetsPrint(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oWUSubContracts.Count() > 0)
                {
                    ExportToExcel(oWUSubContracts, oCompany);
                }
                else
                {
                    throw new Exception(oWUSubContract.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                #region Errormessage

                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sub-Contract List");
                    sheet.Name = "Sub-Contract List";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Sub-Contract_List.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

                #endregion
            }
        }

        private string GetSQL(WUSubContract oWUSubContract)
        {
            bool chkContractDate = false;
            DateTime dtpContractStartDate = DateTime.Now;
            DateTime dtpContractEndDate = DateTime.Now;
            bool chkProdStartDate = false;
            DateTime dtpProdStartDate = DateTime.Now;
            DateTime dtpProdEndDate = DateTime.Now;
            bool chkProdCompleteDate = false;
            DateTime dtpProdCompleteStartDate = DateTime.Now;
            DateTime dtpProdCompleteEndDate = DateTime.Now;
            bool chkRate = false;
            string txtRateStartPerMeasurementUnit = "";
            string txtRateEndPerMeasurementUnit = "";
            bool chkTotalAmount = false;
            string txtStartTotalAmount = "";
            string txtEndTotalAmount = "";
            string YarnName = "";
            int nWarpWeftTypeInt = 0;
            int nReportLayoutInt = 0;

            if (oWUSubContract.ErrorMessage != null)
            {
                chkContractDate = Convert.ToBoolean(oWUSubContract.ErrorMessage.Split('~')[0]);
                dtpContractStartDate = Convert.ToDateTime(oWUSubContract.ErrorMessage.Split('~')[1]);
                dtpContractEndDate = Convert.ToDateTime(oWUSubContract.ErrorMessage.Split('~')[2]);
                chkProdStartDate = Convert.ToBoolean(oWUSubContract.ErrorMessage.Split('~')[3]);
                dtpProdStartDate = Convert.ToDateTime(oWUSubContract.ErrorMessage.Split('~')[4]);
                dtpProdEndDate = Convert.ToDateTime(oWUSubContract.ErrorMessage.Split('~')[5]);
                chkProdCompleteDate = Convert.ToBoolean(oWUSubContract.ErrorMessage.Split('~')[6]);
                dtpProdCompleteStartDate = Convert.ToDateTime(oWUSubContract.ErrorMessage.Split('~')[7]);
                dtpProdCompleteEndDate = Convert.ToDateTime(oWUSubContract.ErrorMessage.Split('~')[8]);
                chkRate = Convert.ToBoolean(oWUSubContract.ErrorMessage.Split('~')[9]);
                txtRateStartPerMeasurementUnit = Convert.ToString(oWUSubContract.ErrorMessage.Split('~')[10]);
                txtRateEndPerMeasurementUnit = Convert.ToString(oWUSubContract.ErrorMessage.Split('~')[11]);
                chkTotalAmount = Convert.ToBoolean(oWUSubContract.ErrorMessage.Split('~')[12]);
                txtStartTotalAmount = Convert.ToString(oWUSubContract.ErrorMessage.Split('~')[13]);
                txtEndTotalAmount = Convert.ToString(oWUSubContract.ErrorMessage.Split('~')[14]);
                YarnName = Convert.ToString(oWUSubContract.ErrorMessage.Split('~')[15]);
                nWarpWeftTypeInt = Convert.ToInt32(oWUSubContract.ErrorMessage.Split('~')[16]);
                nReportLayoutInt = Convert.ToInt32(oWUSubContract.ErrorMessage.Split('~')[16]);
            }

            string sReturn1 = "SELECT * FROM View_WUSubContract AS HH";
            string sReturn = "";

            #region Job No
            if (!string.IsNullOrEmpty(oWUSubContract.JobNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.JobNo LIKE '%" + oWUSubContract.JobNo + "%'";
            }
            #endregion

            #region Contract Date
            if (chkContractDate)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.ContractDate, 106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtpContractStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtpContractEndDate.ToString("dd MMM yyyy") + "', 106))";
            }
            #endregion

            #region Supplier Name
            if (!string.IsNullOrEmpty(oWUSubContract.SupplierName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.SupplierID IN (" + oWUSubContract.SupplierName + ")";
            }
            #endregion

            #region Order Type
            if ((EnumWUOrderType)oWUSubContract.OrderTypeInt != EnumWUOrderType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.OrderType = " + oWUSubContract.OrderTypeInt;
            }
            #endregion

            #region Payment Mode
            if ((EnumInvoicePaymentMode)oWUSubContract.PaymentModeInt != EnumInvoicePaymentMode.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.PaymentMode = " + oWUSubContract.PaymentModeInt;
            }
            #endregion

            #region Transportation
            if ((EnumTransportation)oWUSubContract.TransportationInt != EnumTransportation.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.Transportation = " + oWUSubContract.TransportationInt;
            }
            #endregion

            #region Contract By
            if (oWUSubContract.ContractBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ContractBy = " + oWUSubContract.ContractBy;
            }
            #endregion

            #region SO No
            if (!string.IsNullOrEmpty(oWUSubContract.SONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.SONo LIKE '%" + oWUSubContract.SONo + "%'";
            }
            #endregion

            #region Buyer Name
            if (!string.IsNullOrEmpty(oWUSubContract.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BuyerID IN (" + oWUSubContract.BuyerName + ")";
            }
            #endregion

            #region Style No
            if (!string.IsNullOrEmpty(oWUSubContract.StyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.StyleNo LIKE '%" + oWUSubContract.StyleNo + "%'";
            }
            #endregion

            #region Fabric Type Name
            if (!string.IsNullOrEmpty(oWUSubContract.FabricTypeName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.FabricTypeID IN (" + oWUSubContract.FabricTypeName + ")";
            }
            #endregion

            #region Prod Start Date
            if (chkProdStartDate)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.ProdStartDate, 106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtpProdStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtpProdEndDate.ToString("dd MMM yyyy") + "', 106))";
            }
            #endregion

            #region Weave Design Name
            if (!string.IsNullOrEmpty(oWUSubContract.WeaveDesignName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.WeaveDesignID IN (" + oWUSubContract.WeaveDesignName + ")";
            }
            #endregion

            #region Prod Complete Date
            if (chkProdCompleteDate)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.ProdCompleteDate, 106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtpProdCompleteStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtpProdCompleteEndDate.ToString("dd MMM yyyy") + "', 106))";
            }
            #endregion

            #region Yarn Name
            if (!string.IsNullOrEmpty(YarnName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.WUSubContractID IN ( SELECT WU.WUSubContractID FROM View_WUSubContractYarnConsumption AS WU WHERE WU.YarnName like '%" + YarnName + "%')";
            }
            #endregion

            #region Composition Name
            if (!string.IsNullOrEmpty(oWUSubContract.CompositionName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.CompositionID IN (" + oWUSubContract.CompositionName + ")";
            }
            #endregion

            #region Rate
            if (chkRate && !string.IsNullOrEmpty(txtRateStartPerMeasurementUnit) && !string.IsNullOrEmpty(txtRateEndPerMeasurementUnit))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.Rate BETWEEN " + txtRateStartPerMeasurementUnit + " AND " + txtRateEndPerMeasurementUnit + "";
            }
            #endregion

            #region Warp Weft Type
            if ((EnumWarpWeft)nWarpWeftTypeInt != EnumWarpWeft.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.WUSubContractID IN ( SELECT WU.WUSubContractID FROM WUSubContractYarnConsumption AS WU WHERE WU.WarpWeftType = " + nWarpWeftTypeInt + ")";
            }
            #endregion

            #region Total Amount
            if (chkTotalAmount && !string.IsNullOrEmpty(txtStartTotalAmount) && !string.IsNullOrEmpty(txtEndTotalAmount))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.TotalAmount BETWEEN " + txtStartTotalAmount + " AND " + txtEndTotalAmount + "";
            }
            #endregion

            #region Construction
            if (!string.IsNullOrEmpty(oWUSubContract.Construction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.Construction LIKE '%" + oWUSubContract.Construction + "%'";
            }
            #endregion

            #region Currency
            if (oWUSubContract.CurrencyID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.CurrencyID = " + oWUSubContract.CurrencyID;
            }
            #endregion

            #region Measurement Unit
            if (oWUSubContract.MUnitID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.MUnitID = " + oWUSubContract.MUnitID;
            }
            #endregion

            #region Prod Start Comments
            if (!string.IsNullOrEmpty(oWUSubContract.ProdStartComments))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ProdStartComments LIKE '%" + oWUSubContract.ProdStartComments + "%'";
            }
            #endregion

            //#region Report Layout
            //if (oWUSubContract.ReportLayout == EnumReportLayout.DateWiseDetails)
            //{
            //    sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            //    sSQLQuery = "";
            //    sOrderBy = " ORDER BY  GRNDate, GRNID, GRNDetailID ASC";
            //}

            //else if (oWUSubContract.ReportLayout == EnumReportLayout.PartyWiseDetails)
            //{
            //    sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            //    sSQLQuery = "";
            //    sOrderBy = " ORDER BY  ContractorID, GRNID, GRNDetailID ASC";
            //}
            //else
            //{
            //    sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            //    sSQLQuery = "";
            //    sOrderBy = " ORDER BY ProductID, GRNID, GRNDetailID ASC";
            //}
            //#endregion

            sReturn = sReturn1 + sReturn + " ORDER BY HH.WUSubContractID ASC";
            return sReturn;
        }

        private void ExportToExcel(List<WUSubContract> oWUSubContracts, Company oCompany)
        {
            int rowIndex = 2;
            int colIndex = 0;
            ExcelRange cell;
            Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sub-Contract List");
                sheet.Name = "Sub-Contract List";
                sheet.View.FreezePanes(4, 3);

                #region Declare Column
                sheet.Column(++colIndex).Width = 8;  //SL
                sheet.Column(++colIndex).Width = 15; //Job No
                sheet.Column(++colIndex).Width = 20; //Contract Date
                sheet.Column(++colIndex).Width = 16; //Contract Status
                sheet.Column(++colIndex).Width = 30; //Supplier
                sheet.Column(++colIndex).Width = 30; //Buyer
                sheet.Column(++colIndex).Width = 20; //Composition
                sheet.Column(++colIndex).Width = 20; //Construction
                sheet.Column(++colIndex).Width = 25; //Fabric Type
                sheet.Column(++colIndex).Width = 20; //Weave Design
                sheet.Column(++colIndex).Width = 10; //M.Unit
                sheet.Column(++colIndex).Width = 15; //Order Qty
                sheet.Column(++colIndex).Width = 10; //Currency
                sheet.Column(++colIndex).Width = 15; //Rate/Unit
                sheet.Column(++colIndex).Width = 15; //Amount
                sheet.Column(++colIndex).Width = 20; //Approved By
                sheet.Column(++colIndex).Width = 20; //Yarn Challan Status
                sheet.Column(++colIndex).Width = 20; //Fabric Rcv Status

                #endregion

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion

                #region Column Header
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Job No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Contract Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Contract Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Supplier"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Buyer"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Composition"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Construction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Fabric Type"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Weave Design"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Currency"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Rate/Unit"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Yarn Challan Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Fabric Rcv Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                #endregion

                #region Report Body

                int nCount = 0;
                //double nGrandTotal = 0;
                var nStartRow = rowIndex;
                foreach (WUSubContract oItem in oWUSubContracts)
                {
                    colIndex = 1;
                    nCount++;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount.ToString();
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FullJobNoSt;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractDate; cell.Style.Numberformat.Format = "dd MMM yyyy";
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContractStatus;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    // nGrandTotal = nGrandTotal + oItem.Amount;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SupplierName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BuyerName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CompositionName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Construction;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricTypeName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.WeaveDesignName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.MUSymbol;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OrderQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CUSymbol;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Rate; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ApprovedByName;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.YarnChallanStatusSt;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FabricRcvStatusSt;
                    fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;

                }
                var nEndRow = rowIndex - 1;
                cell = sheet.Cells[rowIndex, 1, rowIndex, 11]; cell.Merge = true; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                colIndex = 12;
                var sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                var sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 18]; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Sub-Contract_List.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
















        //#region Excel
        //private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        //{
        //    return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        //}
        //private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        //{
        //    ExcelRange cell;
        //    OfficeOpenXml.Style.Border border;

        //    cell = sheet.Cells[nRowIndex, nStartCol++];
        //    if (IsNumber)
        //        cell.Value = Convert.ToDouble(sVal);
        //    else
        //        cell.Value = sVal;
        //    cell.Style.Font.Bold = IsBold;
        //    cell.Style.WrapText = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

        //    if (IsNumber)
        //    {
        //        cell.Style.Numberformat.Format = _sFormatter;
        //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    }
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //    return cell;
        //}
        //public void ExportToExcelWUSubContractPrint(double ts)
        //{
        //    int nGRNID = -999;
        //    string Header = "", HeaderColumn = "";

        //    Company oCompany = new Company();
        //    WUSubContractPrint oWUSubContractPrint = new WUSubContractPrint();
        //    try
        //    {
        //        _sErrorMesage = "";
        //        _oWUSubContractPrints = new List<WUSubContractPrint>();
        //        oWUSubContractPrint = (WUSubContractPrint)Session[SessionInfo.ParamObj];
        //        string sSQL = this.GetSQL(oWUSubContractPrint);
        //        _oWUSubContractPrints = WUSubContractPrint.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (_oWUSubContractPrints.Count <= 0)
        //        {
        //            _sErrorMesage = "There is no data for print!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _oWUSubContractPrints = new List<WUSubContractPrint>();
        //        _sErrorMesage = ex.Message;
        //    }

        //    bool _bIsRateView = false;
        //    List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
        //    oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

        //    oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
        //    if (oAuthorizationRoleMapping.Count > 0)
        //    {
        //        _bIsRateView = true;
        //    }

        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
        //    if (oWUSubContractPrint.BUID > 0)
        //    {
        //        BusinessUnit oBU = new BusinessUnit();
        //        oBU = oBU.Get(oWUSubContractPrint.BUID, (int)Session[SessionInfo.currentUserID]);
        //        oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
        //    }

        //    if (_sErrorMesage == "")
        //    {
        //        #region Header
        //        List<TableHeader> table_header = new List<TableHeader>();
        //        table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "GRN No", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "GL Date", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "GRN Status", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Style", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Ref. No", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Challan No", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Store Name", Width = 35f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Supplier Name", Width = 45f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Brand Name", Width = 20f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Approved By", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "LC No", Width = 15f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "GRN Date", Width = 30f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "M. Unit", Width = 11f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Ref Qty", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "GRN Qty", Width = 25f, IsRotate = false });
        //        table_header.Add(new TableHeader { Header = "Extra Qty", Width = 25f, IsRotate = false });


        //        if (_bIsRateView)
        //        {
        //            table_header.Add(new TableHeader { Header = "Rate", Width = 25f, IsRotate = false });
        //            table_header.Add(new TableHeader { Header = "Amount", Width = 25f, IsRotate = false });
        //        }
        //        #endregion

        //        #region Layout Wise Header
        //        if (oWUSubContractPrint.ReportLayout == EnumReportLayout.ProductWise)
        //        {
        //            Header = "Product Wise"; HeaderColumn = "Product Name : ";
        //        }
        //        #endregion

        //        #region Export Excel
        //        int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
        //        ExcelRange cell; ExcelFill fill;
        //        OfficeOpenXml.Style.Border border;

        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            excelPackage.Workbook.Properties.Author = "ESimSol";
        //            excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //            #region Detail Data
        //            var sheet = excelPackage.Workbook.Worksheets.Add("GRN Register");
        //            sheet.Name = "GRN Register";

        //            foreach (TableHeader listItem in table_header)
        //            {
        //                sheet.Column(nStartCol++).Width = listItem.Width;
        //            }

        //            #region Report Header
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
        //            cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
        //            cell.Value = "GRN Register (" + Header + ") "; cell.Style.Font.Bold = true;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Address & Date
        //            cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
        //            cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
        //            cell.Value = ""; cell.Style.Font.Bold = false;
        //            cell.Style.WrapText = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            nRowIndex = nRowIndex + 1;
        //            #endregion

        //            #region Group By Layout Wise
        //            var data = _oWUSubContractPrints.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
        //            {
        //                HeaderName = key.ProductName,
        //                TotalQty = grp.Sum(x => x.ReceivedQty),
        //                TotalAmount = grp.Sum(x => x.Amount),
        //                Results = grp.ToList()
        //            });

        //            //if (oWUSubContractPrint.ReportLayout == EnumReportLayout.ProductWise)
        //            //{
        //            //    data = _oWUSubContractPrints.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
        //            //    {
        //            //        HeaderName = key.ProductName,
        //            //        TotalQty = grp.Sum(x => x.Qty),
        //            //        TotalAmount = grp.Sum(x => x.Amount),
        //            //        Results = grp.ToList()
        //            //    });
        //            //}
        //            //else if (oWUSubContractPrint.ReportLayout == EnumReportLayout.DateWiseDetails)
        //            //{
        //            //    data = _oWUSubContractPrints.GroupBy(x => new { x.DateofInvoiceSt }, (key, grp) => new
        //            //    {
        //            //        HeaderName = key.DateofInvoiceSt,
        //            //        TotalQty = grp.Sum(x => x.Qty),
        //            //        TotalAmount = grp.Sum(x => x.Amount),
        //            //        Results = grp.ToList()
        //            //    });
        //            //}
        //            #endregion

        //            string sCurrencySymbol = "";
        //            #region Data
        //            foreach (var oItem in data)
        //            {
        //                nRowIndex++;

        //                nStartCol = 2;
        //                FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

        //                nRowIndex++;
        //                foreach (TableHeader listItem in table_header)
        //                {
        //                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                }

        //                nGRNID = 0;
        //                nRowIndex++; int nCount = 0, nRowSpan = 0;
        //                foreach (var obj in oItem.Results)
        //                {
        //                    #region Product Wise Merge
        //                    if (nGRNID != obj.GRNID)
        //                    {
        //                        if (nCount > 0)
        //                        {
        //                            nStartCol = 16;
        //                            FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

        //                            _sFormatter = " #,##0;(#,##0)";
        //                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.ReceivedQty).ToString(), true, true);
        //                            FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //                            if (_bIsRateView)
        //                            {
        //                                FillCell(sheet, nRowIndex, ++nStartCol, "", false);//Rate
        //                                FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.Amount).ToString(), true, true);//amount
        //                            }
        //                            nRowIndex++;
        //                        }

        //                        nStartCol = 2;
        //                        nRowSpan = oItem.Results.Where(GRNR => GRNR.GRNID == obj.GRNID && GRNR.ProductID == obj.ProductID).ToList().Count;

        //                        FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.GRNNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.GLDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.GRNStatusSt.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.StyleNo.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.RefObjectNo.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.ChallanNo.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.ColorName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.StoreName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.SupplierName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.BrandName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                        FillCellMerge(ref sheet, obj.BuyerName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
        //                    }
        //                    #endregion

        //                    nStartCol = 14;

        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.ApprovedByName, false);
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo, false);
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.GRNDateSt, false);
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.MUSymbol, false);
        //                    _sFormatter = " #,##0;(#,##0)";
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.RefQty.ToString(), true);
        //                    _sFormatter = " #,##0;(#,##0)";
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.ReceivedQty.ToString(), true);
        //                    _sFormatter = " #,##0;(#,##0)";
        //                    FillCell(sheet, nRowIndex, nStartCol++, obj.ExtraQty < 0 ? "0" : obj.ExtraQty.ToString(), true);

        //                    if (_bIsRateView)
        //                    {
        //                        FillCell(sheet, nRowIndex, nStartCol++, obj.UnitPrice.ToString(), true);
        //                        FillCell(sheet, nRowIndex, nStartCol++, obj.Amount.ToString(), true);
        //                    }
        //                    nRowIndex++;

        //                    nGRNID = obj.GRNID;
        //                    sCurrencySymbol = obj.CurrencySymbol;
        //                }

        //                nStartCol = 16;
        //                FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
        //                _sFormatter = " #,##0;(#,##0)";
        //                FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.ReceivedQty).ToString(), true, true);
        //                FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //                if (_bIsRateView)
        //                {
        //                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);//Rate
        //                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.GRNID == nGRNID).Sum(x => x.Amount).ToString(), true, true);//Amount
        //                }
        //                nRowIndex++;

        //                nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
        //                FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 16, true, ExcelHorizontalAlignment.Right);
        //                FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalQty.ToString(), true, true);
        //                FillCell(sheet, nRowIndex, ++nStartCol, "", false, true);
        //                if (_bIsRateView)
        //                {
        //                    FillCell(sheet, nRowIndex, ++nStartCol, "", false, true); //rate 
        //                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.TotalAmount.ToString(), true, true); //amount
        //                }
        //                nRowIndex++;
        //            }

        //            #region Grand Total
        //            nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
        //            FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 16, true, ExcelHorizontalAlignment.Right);
        //            FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalQty).ToString(), true, true);
        //            FillCell(sheet, nRowIndex, ++nStartCol, "", false);
        //            if (_bIsRateView)
        //            {
        //                FillCell(sheet, nRowIndex, ++nStartCol, "", false);//Rate
        //                FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.TotalAmount).ToString(), true, true); //Amount
        //            }
        //            nRowIndex++;
        //            #endregion
        //            #endregion
        //            #endregion
        //            #region Summary
        //            var Summarysheet = excelPackage.Workbook.Worksheets.Add("Update Summary");
        //            Summarysheet.Name = "Update Summary";

        //           int  colIndex = 1;
        //            Summarysheet.Column(++colIndex).Width = 8; //SL
        //            Summarysheet.Column(++colIndex).Width = 30; //Product Name
        //            Summarysheet.Column(++colIndex).Width = 25; //Supplier Name
        //            Summarysheet.Column(++colIndex).Width = 15; //M Unit
        //            Summarysheet.Column(++colIndex).Width = 15; //Grn Qty
        //            Summarysheet.Column(++colIndex).Width = 13; //Rate(BDT)
        //            Summarysheet.Column(++colIndex).Width = 13; //Amount(BDT)
        //            colIndex = 1;
        //            int rowIndex = 2;

        //            #region Address & Date
        //            cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true;
        //            cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            rowIndex++;

        //            cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true;
        //            cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
        //            cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            rowIndex = rowIndex + 2;
        //            #endregion

        //            colIndex = 1;

        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Supplier Name"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "GRN Qty"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Rate("+oCompany.CurrencyName+")"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = "Amount(" + oCompany.CurrencyName + ")"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //            var dataSummary = _oWUSubContractPrints.GroupBy(x => new { x.ProductName, x.SupplierName }, (key, grp) => new
        //            {
        //                ProductName = key.ProductName,
        //                SupplierName = key.SupplierName,
        //                TotalQty = grp.Sum(x => x.ReceivedQty),
        //                TotalAmount = grp.Sum(x => x.Amount),
        //                UnitName=grp.ToList().Select(x=>x.MUName).FirstOrDefault(),
        //                Results = grp.ToList()
        //            });

        //            dataSummary = dataSummary.OrderBy(x => x.ProductName);
        //            rowIndex++;
        //            colIndex = 1;
        //             int Count = 0;
        //            int nStartRow = rowIndex;

        //            foreach (var oItem in dataSummary)
        //            {
        //                colIndex = 1;
        //                Count++;
        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = Count; cell.Style.Font.Bold = true;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.SupplierName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.TotalQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.TotalAmount / oItem.TotalQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = Summarysheet.Cells[rowIndex, ++colIndex]; cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                rowIndex++;
        //            }


        //            cell = Summarysheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            string sStartCell = Global.GetExcelCellName(nStartRow, 8);
        //           string  sEndCell = Global.GetExcelCellName(rowIndex - 1, 8);

        //           cell = Summarysheet.Cells[rowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold =true; cell.Style.Numberformat.Format = "#,##,##0;(#,##,##0)";
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


        //            cell = Summarysheet.Cells[1, 1, rowIndex, 10];
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.White);
        //            #endregion
        //            cell = sheet.Cells[1, 1, nRowIndex, 17];
        //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
        //            fill.BackgroundColor.SetColor(Color.White);




        //            Response.ClearContent();
        //            Response.BinaryWrite(excelPackage.GetAsByteArray());
        //            Response.AddHeader("content-disposition", "attachment; filename=WUSubContractPrint(" + Header + ").xlsx");
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.Flush();
        //            Response.End();
        //        }
        //        #endregion
        //    }
        //}
        //private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        //{
        //    ExcelRange cell;
        //    OfficeOpenXml.Style.Border border;

        //    cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
        //    cell.Merge = true;
        //    cell.Value = sVal;
        //    cell.Style.Font.Bold = false;
        //    cell.Style.WrapText = true;
        //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        //}
        //private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        //{
        //    FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        //}
        //private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        //{
        //    ExcelRange cell;
        //    OfficeOpenXml.Style.Border border;

        //    cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
        //    cell.Merge = true;
        //    cell.Value = sVal;
        //    cell.Style.Font.Bold = isBold;
        //    cell.Style.WrapText = true;
        //    cell.Style.HorizontalAlignment = HoriAlign;
        //    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //}

        //#endregion



        #region Support Functions
        //private string GetSQL(WUSubContract oWUSubContract)
        //{
        //    _sDateRange = "";
        //    string sSearchingData = oWUSubContract.SearchingData;
        //    EnumCompareOperator eGRNDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
        //    DateTime dGRNStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
        //    DateTime dGRNEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);

        //    EnumCompareOperator eGLDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
        //    DateTime dGLDateStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
        //    DateTime dGLDateEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);

        //    EnumCompareOperator eApprovedDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
        //    DateTime dApprovedStartDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
        //    DateTime dApprovedEndDate = Convert.ToDateTime(sSearchingData.Split('~')[8]);

        //    EnumCompareOperator ePIAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
        //    double nPIAmountStsrt = Convert.ToDouble(sSearchingData.Split('~')[10]);
        //    double nPIAmountEnd = Convert.ToDouble(sSearchingData.Split('~')[11]);

        //    int nRefType = Convert.ToInt32(sSearchingData.Split('~')[12]);
        //    string sRefNo = Convert.ToString(sSearchingData.Split('~')[13]);


        //    string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

        //    #region BusinessUnit
        //    if (oWUSubContractPrint.BUID > 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " BUID =" + oWUSubContractPrint.BUID.ToString();
        //    }
        //    #endregion

        //    #region GRNNo
        //    if (oWUSubContractPrint.GRNNo != null && oWUSubContractPrint.GRNNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " GRNNo LIKE'%" + oWUSubContractPrint.GRNNo + "%'";
        //    }
        //    #endregion

        //    #region ChallanNo
        //    if (oWUSubContractPrint.ChallanNo != null && oWUSubContractPrint.ChallanNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ChallanNo LIKE'%" + oWUSubContractPrint.ChallanNo + "%'";
        //    }
        //    #endregion

        //    #region LotNo
        //    if (oWUSubContractPrint.LotNo != null && oWUSubContractPrint.LotNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " LotNo LIKE'%" + oWUSubContractPrint.LotNo + "%'";
        //    }
        //    #endregion

        //    #region ColorName
        //    if (oWUSubContractPrint.ColorName != null && oWUSubContractPrint.ColorName != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ColorName LIKE'%" + oWUSubContractPrint.ColorName + "%'";
        //    }
        //    #endregion

        //    #region ApprovedBy
        //    if (oWUSubContractPrint.ApproveBy != 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ApproveBy =" + oWUSubContractPrint.ApproveBy.ToString();
        //    }
        //    #endregion

        //    #region GRNStatus
        //    if (oWUSubContractPrint.GRNStatus != EnumGRNStatus.Initialize)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " GRNStatus =" + ((int)oWUSubContractPrint.GRNStatus).ToString();
        //    }
        //    #endregion

        //    #region Remarks
        //    if (oWUSubContractPrint.Remarks != null && oWUSubContractPrint.Remarks != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " GRN.Remarks LIKE'%" + oWUSubContractPrint.Remarks + "%'";
        //    }
        //    #endregion

        //    #region Supplier
        //    if (oWUSubContractPrint.SupplierName != null && oWUSubContractPrint.SupplierName != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ContractorID IN(" + oWUSubContractPrint.SupplierName + ")";
        //    }
        //    #endregion

        //    #region Style
        //    if (oWUSubContractPrint.StyleNo != null && oWUSubContractPrint.StyleNo != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " StyleID IN(" + oWUSubContractPrint.StyleNo + ")";
        //    }
        //    #endregion

        //    #region ProductType
        //    if (oWUSubContractPrint.ProductType != EnumProductNature.Dyeing)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        //sWhereCluse = sWhereCluse + "  RefType =  "+(int)EnumGRNType.ImportPI+" AND  RefObjectID IN (SELECT ImportPIDetailID FROM ImportPIDetail WHERE  ImportPIID IN (SELECT ImportPIID FROM ImportPI WHERE  ProductType =" + ((int)oWUSubContractPrint.ProductType).ToString() + "))";
        //        sWhereCluse = sWhereCluse + " ProductID in ( SELECT ProductID FROM Product AS HH WHERE HH.Activity = 1 and  HH.ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + oWUSubContractPrint.BUID + ", " +(int)oWUSubContractPrint.ProductType+ "))) ";
        //    }
        //    #endregion

        //    #region Store 
        //    if (oWUSubContractPrint.StoreName != null && oWUSubContractPrint.StoreName != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " StoreID IN(" + oWUSubContractPrint.StoreName + ")";
        //    }
        //    #endregion

        //    #region Product
        //    if (oWUSubContractPrint.ProductName != null && oWUSubContractPrint.ProductName != "")
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " ProductID IN(" + oWUSubContractPrint.ProductName + ")";
        //    }
        //    #endregion

        //    #region GRN Date
        //    if (eGRNDate != EnumCompareOperator.None)
        //    {
        //        if (eGRNDate == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "GRN Date @ " + dGRNStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (eGRNDate == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "GRN Date Not Equal @ " + dGRNStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (eGRNDate == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "GRN Date Greater Then @ " + dGRNStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (eGRNDate == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "GRN Date Smaller Then @ " + dGRNStartDate.ToString("dd MMM yyyy");
        //        }
        //        else if (eGRNDate == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNEndDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "GRN Date Between " + dGRNStartDate.ToString("dd MMM yyyy") + " To " + dGRNEndDate.ToString("dd MMM yyyy");
        //        }
        //        else if (eGRNDate == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGRNEndDate.ToString("dd MMM yyyy") + "', 106))";
        //            _sDateRange = "GRN Date NOT Between " + dGRNStartDate.ToString("dd MMM yyyy") + " To " + dGRNEndDate.ToString("dd MMM yyyy");
        //        }
        //    }
        //    #endregion

        //    #region GLDate Date
        //    if (eGLDate != EnumCompareOperator.None)
        //    {
        //        if (eGLDate == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eGLDate == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eGLDate == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eGLDate == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eGLDate == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eGLDate == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),GLDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGLDateEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //    }
        //    #endregion

        //    #region Approved Date
        //    if (eApprovedDate != EnumCompareOperator.None)
        //    {
        //        if (eApprovedDate == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApprovedDate == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApprovedDate == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApprovedDate == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApprovedDate == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //        else if (eApprovedDate == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ApproveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dApprovedEndDate.ToString("dd MMM yyyy") + "', 106))";
        //        }
        //    }
        //    #endregion

        //    #region Amount
        //    if (ePIAmount != EnumCompareOperator.None)
        //    {
        //        if (ePIAmount == EnumCompareOperator.EqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " Amount = " + nPIAmountStsrt.ToString("0.00");
        //        }
        //        else if (ePIAmount == EnumCompareOperator.NotEqualTo)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " Amount != " + nPIAmountStsrt.ToString("0.00"); ;
        //        }
        //        else if (ePIAmount == EnumCompareOperator.GreaterThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " Amount > " + nPIAmountStsrt.ToString("0.00"); ;
        //        }
        //        else if (ePIAmount == EnumCompareOperator.SmallerThan)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " Amount < " + nPIAmountStsrt.ToString("0.00"); ;
        //        }
        //        else if (ePIAmount == EnumCompareOperator.Between)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " Amount BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
        //        }
        //        else if (ePIAmount == EnumCompareOperator.NotBetween)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " Amount NOT BETWEEN " + nPIAmountStsrt.ToString("0.00") + " AND " + nPIAmountEnd.ToString("0.00");
        //        }
        //    }
        //    #endregion

        //    #region RefType
        //    if (nRefType != 0)
        //    {
        //        Global.TagSQL(ref sWhereCluse);
        //        sWhereCluse = sWhereCluse + " RefType = " + nRefType ;
        //    }
        //    #endregion

        //    #region RefNo
        //    if (sRefNo != null && sRefNo != "")
        //    {
        //        if (nRefType != 0)
        //        {
        //            Global.TagSQL(ref sWhereCluse);
        //            sWhereCluse = sWhereCluse + " GRN.RefObjectID IN (" + sRefNo + ")";
        //        }
        //    }
        //    #endregion

        //    #region Report Layout
        //   if (oWUSubContractPrint.ReportLayout == EnumReportLayout.DateWiseDetails)
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "";
        //        sOrderBy = " ORDER BY  GRNDate, GRNID, GRNDetailID ASC";
        //    }

        //    else if (oWUSubContractPrint.ReportLayout == EnumReportLayout.PartyWiseDetails)
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "";
        //        sOrderBy = " ORDER BY  ContractorID, GRNID, GRNDetailID ASC";
        //    }
        //    else
        //    {
        //        sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
        //        sSQLQuery = "";
        //        sOrderBy = " ORDER BY ProductID, GRNID, GRNDetailID ASC";
        //    }
        //    #endregion

        //    sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
        //    return sSQLQuery;
        //}

        #endregion
    }
}

