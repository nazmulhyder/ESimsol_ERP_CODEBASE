using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class RawMaterialOutController : Controller
    {
        #region Declaration
        string _sDateRange;        
        string _sErrorMesage;
        List<RMOutRegister> _oRMOutRegisters = new List<RMOutRegister>();        
        #endregion

        #region Actions
        public ActionResult ViewRawMaterialOut(int RMRequisitionID, int ProductionSheetID, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RawMaterialOut).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ProductionSheet oProductionSheet = new ProductionSheet();
            RMRequisition oRMRequisition = new RMRequisition();
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.RawMaterialOut, EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.ProductionSheetID = ProductionSheetID;

            ViewBag.RMRequisition = oRMRequisition.Get(RMRequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_RMRequisitionSheet AS PS WHERE PS.RMRequisitionID = " + RMRequisitionID;
            ViewBag.RMRequisitionSheets = RMRequisitionSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oProductionSheet);
        }

        public ActionResult ViewRawMaterialReturn(int RMRequisitionID, int ProductionSheetID, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.RawMaterialOut).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ProductionSheet oProductionSheet = new ProductionSheet();
            RMRequisition oRMRequisition = new RMRequisition();
            ViewBag.BUID = buid;
            ViewBag.ProductionSheetID = ProductionSheetID;
            ViewBag.RMRequisition = oRMRequisition.Get(RMRequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sSQL = "SELECT * FROM View_RMRequisitionSheet AS PS WHERE PS.RMRequisitionID = " + RMRequisitionID;
            ViewBag.RMRequisitionSheets = RMRequisitionSheet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            sSQL = "SELECT  *, ISNULL((SELECT SUM(ISNULL(IT.Qty,0)) FROM ITransaction AS IT WHERE IT.TriggerParentType = " + (int)EnumTriggerParentsType.RawMaterial_Return + " AND IT.TriggerParentID=HH.TriggerParentID),0)AS AlreadyReturnQty FROM View_ITransaction AS HH WHERE HH.TriggerParentType = " + (int)EnumTriggerParentsType.ProductionRecipe + " AND HH.TriggerParentID IN (SELECT RRM.RMRequisitionMaterialID FROM RMRequisitionMaterial AS RRM WHERE RMRequisitionID = " + RMRequisitionID + " AND RRM.ProductionSheetID = " + ProductionSheetID + ")  Order bY HH.LotID, HH.ITransactionID";
            ViewBag.ITransactions = ITransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oProductionSheet);
        }
                
        [HttpPost]
        public JsonResult GetITransaction(ProductionRecipe oProductionRecipe)
        {
            List<Lot> oLots = new List<Lot>();            
            List<ITransaction> oITransactions = new List<ITransaction>();
            try
            {
                string sSQL = "SELECT  *, ISNULL((SELECT SUM(ISNULL(IT.Qty,0)) FROM ITransaction AS IT WHERE IT.TriggerParentType = " + (int)EnumTriggerParentsType.RawMaterial_Return + " AND IT.TriggerParentID=HH.TriggerParentID),0)AS AlreadyReturnQty FROM View_ITransaction AS HH WHERE HH.TriggerParentType = " + (int)EnumTriggerParentsType.ProductionRecipe + " AND HH.TriggerParentID IN (SELECT RRM.RMRequisitionMaterialID FROM RMRequisitionMaterial AS RRM WHERE RMRequisitionID = " +oProductionRecipe.RMRequisitionID + " AND RRM.ProductionSheetID = " +oProductionRecipe.ProductionSheetID + ")  Order bY HH.LotID, HH.ITransactionID";
                oITransactions = ITransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ITransaction oITransaction = new ITransaction();
                oITransaction.ErrorMessage = ex.Message;
                oITransactions.Add(oITransaction);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITransactions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReceiveReturnQty(RMRequisitionMaterial oRMRequisitionMaterial)
        {
            try
            {
                if (!oRMRequisitionMaterial.ITransactions.Any())
                {
                    throw new Exception("No items found for raw material Return.");
                }
                if (oRMRequisitionMaterial.ITransactions.Where(x => x.ReturnQty <= 0).Any())
                {
                    throw new Exception("No working unit found for product " + oRMRequisitionMaterial.ITransactions.Where(x => x.ReturnQty <= 0).FirstOrDefault().ProductName);
                }
                oRMRequisitionMaterial = oRMRequisitionMaterial.ReceiveReturnQty((int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT  *, ISNULL((SELECT SUM(ISNULL(IT.Qty,0)) FROM ITransaction AS IT WHERE IT.TriggerParentType = " + (int)EnumTriggerParentsType.RawMaterial_Return + " AND IT.TriggerParentID=HH.TriggerParentID),0)AS AlreadyReturnQty FROM View_ITransaction AS HH WHERE HH.TriggerParentType = " + (int)EnumTriggerParentsType.ProductionRecipe + " AND HH.TriggerParentID IN (SELECT RRM.RMRequisitionMaterialID FROM RMRequisitionMaterial AS RRM WHERE RMRequisitionID = " + oRMRequisitionMaterial.RMRequisitionID + " AND RRM.ProductionSheetID = " + oRMRequisitionMaterial.ProductionSheetID + ")  Order bY HH.LotID, HH.ITransactionID";
                 oRMRequisitionMaterial.ITransactions = ITransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitionMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public JsonResult GetsProductionRecipe(ProductionRecipe oProductionRecipe)
        {
            List<Lot> oLots = new List<Lot>();
            RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
            List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
            try
            {
                string sSQL = "SELECT * FROM View_RMRequisitionMaterial AS HH WHERE HH.RMRequisitionID = " + oProductionRecipe.RMRequisitionID.ToString() + " AND HH.ProductionSheetID = " + oProductionRecipe.ProductionSheetID.ToString() + " AND HH.YetToOutQty>0 ORDER BY RMRequisitionMaterialID ASC";
                oRMRequisitionMaterials = RMRequisitionMaterial.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                sSQL = "SELECT * FROM View_Lot AS Lot WHERE Lot.Balance>0 AND Lot.WorkingUnitID =" + oProductionRecipe.WUID.ToString() + " AND Lot.BUID =" + oProductionRecipe.BUID.ToString() + " AND Lot.ProductID IN(SELECT HH.ProductID FROM View_RMRequisitionMaterial AS HH WHERE HH.RMRequisitionID = " + oProductionRecipe.RMRequisitionID.ToString() + " AND HH.ProductionSheetID = " + oProductionRecipe.ProductionSheetID.ToString() + " AND HH.YetToOutQty>0)";
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oLots != null && oLots.Count > 0)
                {
                    foreach (RMRequisitionMaterial oItem in oRMRequisitionMaterials)
                    {
                        oItem.StockBalance = oLots.Where(item => item.ProductID == oItem.ProductID).Sum(item => item.Balance);
                        if (oItem.StockBalance >=oItem.YetToOutQty)
                        {
                            oItem.CurrentOutQty = Math.Round(oItem.YetToOutQty,4);
                        }
                        else
                        {
                            oItem.CurrentOutQty = Math.Round(oItem.StockBalance,4);
                        }
                        
                    }
                }                
                //oRMRequisitionMaterials = RMRequisitionMaterial.Gets( oProductionRecipe.ProductionSheetID, oProductionRecipe.WUID, oProductionRecipe.RMRequisitionID, (int)Session[SessionInfo.currentUserID]);
                //if (oProductionRecipes.Any() && oProductionRecipes.FirstOrDefault().ProductionRecipeID > 0)
                //{
                //    oProductionRecipes.ForEach(x => { x.CurrentOutQty = x.RequisitionWiseYetToOutQty; x.WUID = oProductionRecipe.WUID; });
                //}
            }
            catch (Exception ex)
            {
                oRMRequisitionMaterial = new RMRequisitionMaterial();

                oRMRequisitionMaterial.ErrorMessage = ex.Message;
                oRMRequisitionMaterials.Add(oRMRequisitionMaterial);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitionMaterials);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult CommitRawMaterialOut(RMRequisitionMaterial oRMRequisitionMaterial)
        {
            try
            {
                if (!oRMRequisitionMaterial.RMRequisitionMaterials.Any())
                {
                    throw new Exception("No items found for raw material out.");
                }
                if (oRMRequisitionMaterial.WUID <= 0)
                {
                    throw new Exception("Invalid Store!");
                }
                if (oRMRequisitionMaterial.RMRequisitionMaterials.Where(x => x.CurrentOutQty <= 0).Any())
                {
                    throw new Exception("No working unit found for product " + oRMRequisitionMaterial.RMRequisitionMaterials.Where(x => x.CurrentOutQty <= 0).FirstOrDefault().ProductName);
                }
                oRMRequisitionMaterial = oRMRequisitionMaterial.CommitRawMaterialOut(EnumTriggerParentsType.ProductionRecipe, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRMRequisitionMaterial);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewRMOutRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            RMOutRegister oRMOutRegister = new RMOutRegister();
            
            ViewBag.BUID = buid;          
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(oRMOutRegister);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(RMOutRegister oRMOutRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oRMOutRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintRMOutRegister(double ts)
        {
            RMOutRegister oRMOutRegister = new RMOutRegister();
            try
            {
                _sErrorMesage = "";
                _oRMOutRegisters = new List<RMOutRegister>();
                oRMOutRegister = (RMOutRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oRMOutRegister);
                _oRMOutRegisters = RMOutRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oRMOutRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oRMOutRegisters = new List<RMOutRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oRMOutRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptRMOutRegisters oReport = new rptRMOutRegisters();
                byte[] abytes = oReport.PrepareReport(_oRMOutRegisters, oCompany, _sDateRange);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        #region Print XlX
        public void ExportToExcelRMOutRegister(double ts)
        {
            RMOutRegister oRMOutRegister = new RMOutRegister();
            _oRMOutRegisters = new List<RMOutRegister>();
            oRMOutRegister = (RMOutRegister)Session[SessionInfo.ParamObj];
            string sSQL = this.GetSQL(oRMOutRegister);
            _oRMOutRegisters = RMOutRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            if (_oRMOutRegisters.Count <= 0)
            {
                _sErrorMesage = "There is no data for print!";
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(oRMOutRegister.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

            int nSL = 0;
            #region Export Excel
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 20, nTempCol = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("RM Out Register");
                string sReportHeader = " ";
                #region Report Body & Header
                sReportHeader = "Raw Material Out Register";
                sheet.Column(nTempCol).Width = 15; nTempCol++;//sl      
                sheet.Column(nTempCol).Width = 15; nTempCol++;//Requsition no
                sheet.Column(nTempCol).Width = 15; nTempCol++;//Requsition date 
                sheet.Column(nTempCol).Width = 15; nTempCol++;//Production Sheet No
                sheet.Column(nTempCol).Width = 15; nTempCol++;//RM Code
                sheet.Column(nTempCol).Width = 50; nTempCol++;//RM Name
                sheet.Column(nTempCol).Width = 15; nTempCol++;//RM Unit
                sheet.Column(nTempCol).Width = 15; nTempCol++;//Requsition Qty
                sheet.Column(nTempCol).Width = 15; nTempCol++;//RM Out Qty
                sheet.Column(nTempCol).Width = 15; nTempCol++;//Remaining Qty                
                #endregion
                nEndCol = nTempCol + 1;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = oBU.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = oBU.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data

                #region column title
                nTempCol = 1;
                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Requsition no"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Requsition date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "RM Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "RM Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "RM Unit"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Requsition Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "RM Out Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol++]; cell.Value = "Remaining Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Data
                int nProductionSheetID = 0; int nRMRequsitionID = 0;
                int nTableRow = 0, nRowSpan = 0, nTempRowIndex = 0;                
                foreach (RMOutRegister oItem in _oRMOutRegisters)
                {

                    if (oItem.ProductionSheetID != nProductionSheetID || oItem.RMRequisitionID !=nRMRequsitionID)
                    {  
                        nTableRow++;
                        nRowIndex = nTempRowIndex > 0 ? nTempRowIndex + 1 : nRowIndex;//REset fo next Row print

                        nRowSpan = _oRMOutRegisters.Where(ChallanR => ChallanR.RMRequisitionID == oItem.RMRequisitionID && ChallanR.ProductionSheetID == oItem.ProductionSheetID).ToList().Count;
                        nTempRowIndex = nRowIndex + nRowSpan;
                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1, nTempRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2, nTempRowIndex, 2];
                        cell.Value = oItem.RMRequisitionNo; //+"\n" + oItem.ExportPINo + "\n" + oItem.ContractorName + "\n" + oItem.FinishGoodsName + " : " + Global.MillionFormatActualDigit(oItem.FinisgGoodsQty) + " " + oItem.FGUnitSymbol;
                        cell.Style.Font.Bold = false; cell.Merge = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3, nTempRowIndex, 3]; cell.Value = oItem.RMRequisitionDateSt; cell.Style.Font.Bold = false; cell.Merge = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.RMProductCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.RMProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RMUnitSymbol; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormatActualDigit(oItem.RequisitionQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = Global.MillionFormatActualDigit(oItem.RMOutQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = Global.MillionFormatActualDigit(oItem.RemainingQty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nProductionSheetID = oItem.ProductionSheetID;
                    nRMRequsitionID = oItem.RMRequisitionID;
                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion            
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=RMOutRegister.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion


        }
        #endregion
        private string GetSQL(RMOutRegister oRMOutRegister)
        {
            _sDateRange = "";
            string sSearchingData = oRMOutRegister.SearchingData;
            EnumCompareOperator eRequsitionDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dRequsitionStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dRequsitionEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);            
            string sSQLWhereCluse = "";

            #region BusinessUnit
            if (oRMOutRegister.BUID > 0)
            {
                Global.TagSQL(ref sSQLWhereCluse);
                sSQLWhereCluse = sSQLWhereCluse + " HH.RMRequisitionID IN(SELECT RMR.RMRequisitionID FROM RMRequisition AS RMR WHERE RMR.BUID = " + oRMOutRegister.BUID.ToString() + ")";
            }
            #endregion

            #region RMRequisitionNo
            if (oRMOutRegister.RMRequisitionNo != null && oRMOutRegister.RMRequisitionNo != "")
            {
                Global.TagSQL(ref sSQLWhereCluse);
                sSQLWhereCluse = sSQLWhereCluse + " HH.RMRequisitionID IN(SELECT RMR.RMRequisitionID FROM RMRequisition AS RMR WHERE RMR.RefNo LIKE '%" + oRMOutRegister.RMRequisitionNo + "%')";
            }
            #endregion

            #region ExportPINo
            if (oRMOutRegister.ExportPINo != null && oRMOutRegister.ExportPINo != "")
            {
                Global.TagSQL(ref sSQLWhereCluse);
                sSQLWhereCluse = sSQLWhereCluse + " HH.RMRequisitionID IN(SELECT RMRS.RMRequisitionID FROM RMRequisitionSheet AS RMRS WHERE RMRS.ProductionSheetID IN (SELECT PS.ProductionSheetID FROM ProductionSheet AS PS WHERE PS.PTUUnit2ID IN (SELECT PTU.PTUUnit2ID FROM View_PTUUnit2 AS PTU WHERE PTU.ExportPINo LIKE '%" + oRMOutRegister.ExportPINo + "%')))";
            }
            #endregion

            #region ProductionSheetNo
            if (oRMOutRegister.ProductionSheetNo != null && oRMOutRegister.ProductionSheetNo != "")
            {
                Global.TagSQL(ref sSQLWhereCluse);
                sSQLWhereCluse = sSQLWhereCluse + " HH.RMRequisitionID IN(SELECT RMRS.RMRequisitionID FROM View_RMRequisitionSheet AS RMRS WHERE RMRS.SheetNo LIKE '%" + oRMOutRegister.ProductionSheetNo + "%')";
            }
            #endregion

            #region Buyer
            if (oRMOutRegister.ContractorName != null && oRMOutRegister.ContractorName != "")
            {
                Global.TagSQL(ref sSQLWhereCluse);
                sSQLWhereCluse = sSQLWhereCluse + " HH.RMRequisitionID IN(SELECT RMRS.RMRequisitionID FROM RMRequisitionSheet AS RMRS WHERE RMRS.ProductionSheetID IN (SELECT PS.ProductionSheetID FROM ProductionSheet AS PS WHERE PS.PTUUnit2ID IN (SELECT PTU.PTUUnit2ID FROM View_PTUUnit2 AS PTU WHERE PTU.ContractorID IN(" + oRMOutRegister.ContractorName + "))))";
            }
            #endregion

            #region Product
            if (oRMOutRegister.RMProductName != null && oRMOutRegister.RMProductName != "")
            {
                Global.TagSQL(ref sSQLWhereCluse);
                sSQLWhereCluse = sSQLWhereCluse + " HH.RMRequisitionID IN(SELECT RMRM.RMRequisitionID FROM View_RMRequisitionMaterial AS RMRM WHERE RMRM.ProductID IN (" + oRMOutRegister.RMProductName + "))";

            }
            #endregion

            #region Requsition Date
            if (eRequsitionDate != EnumCompareOperator.None)
            {
                if (eRequsitionDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sSQLWhereCluse);
                    sSQLWhereCluse = sSQLWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.RequisitionDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dRequsitionStartDate.ToString("dd MMM yyyy") + "'', 106))";
                    _sDateRange = "Requsition Date @ " + dRequsitionStartDate.ToString("dd MMM yyyy");
                }
                else if (eRequsitionDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sSQLWhereCluse);
                    sSQLWhereCluse = sSQLWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.RequisitionDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dRequsitionStartDate.ToString("dd MMM yyyy") + "'', 106))";
                    _sDateRange = "Requsition Date Not Equal @ " + dRequsitionStartDate.ToString("dd MMM yyyy");
                }
                else if (eRequsitionDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sSQLWhereCluse);
                    sSQLWhereCluse = sSQLWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.RequisitionDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dRequsitionStartDate.ToString("dd MMM yyyy") + "'', 106))";

                    _sDateRange = "Requsition Date Greater Then @ " + dRequsitionStartDate.ToString("dd MMM yyyy");
                }
                else if (eRequsitionDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sSQLWhereCluse);
                    sSQLWhereCluse = sSQLWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.RequisitionDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), ''" + dRequsitionStartDate.ToString("dd MMM yyyy") + "'', 106))";
                    _sDateRange = "Requsition Date Smaller Then @ " + dRequsitionStartDate.ToString("dd MMM yyyy");
                }
                else if (eRequsitionDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sSQLWhereCluse);
                    sSQLWhereCluse = sSQLWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.RequisitionDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequsitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequsitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requsition Date Between " + dRequsitionStartDate.ToString("dd MMM yyyy") + " To " + dRequsitionEndDate.ToString("dd MMM yyyy");
                }
                else if (eRequsitionDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sSQLWhereCluse);
                    sSQLWhereCluse = sSQLWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),HH.RequisitionDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequsitionStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dRequsitionEndDate.ToString("dd MMM yyyy") + "', 106))";
                    _sDateRange = "Requsition Date NOT Between " + dRequsitionStartDate.ToString("dd MMM yyyy") + " To " + dRequsitionEndDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            return sSQLWhereCluse;
        }
        public Image GetCompanyLogo(Company oCompany)
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


    }

}
