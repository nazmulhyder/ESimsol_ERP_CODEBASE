using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class WUSubContractController : Controller
    {
        #region Declaration
        WUSubContract _oWUSubContract = new WUSubContract();
        List<WUSubContract> _oWUSubContracts = new List<WUSubContract>();
        #endregion

        public ActionResult ViewWUSubContracts(int buid, int MenuId)
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

            return View(_oWUSubContracts);
        }

        public ActionResult ViewWUSubContract(int id, int buid)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            if (id > 0)
            {
                oWUSubContract = oWUSubContract.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<WUSubContractYarnConsumption> oWUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();
                oWUSubContractYarnConsumptions = WUSubContractYarnConsumption.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract.WUSubContractYarnConsumptions = oWUSubContractYarnConsumptions;

                List<WUSubContractTermsCondition> oWUSubContractTermsConditions = new List<WUSubContractTermsCondition>();
                oWUSubContractTermsConditions = WUSubContractTermsCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract.WUSubContractTermsConditions = oWUSubContractTermsConditions;
            }
            else
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

                oWUSubContract = new WUSubContract();
                oWUSubContract.BUID = buid;
                oWUSubContract.BaseCurrencyID = oCompany.BaseCurrencyID;
                oWUSubContract.BCSymbol = oCompany.CurrencySymbol;
            }

            List<Employee> oEmployees = new List<Employee>();
            oEmployees = Employee.Gets(EnumEmployeeDesignationType.Management, (int)Session[SessionInfo.currentUserID]);

            ViewBag.BUID = buid;            
            ViewBag.ContractEmployees = oEmployees;
            ViewBag.PaymentModes = EnumObject.jGets(typeof(EnumInvoicePaymentMode));
            ViewBag.OrderTypes = EnumObject.jGets(typeof(EnumWUOrderType));
            ViewBag.WorkTypes = EnumObject.jGets(typeof(EnumWSCWorkType));
            ViewBag.Transportations = EnumObject.jGets(typeof(EnumTransportation));
            ViewBag.WarpWefts = EnumObject.jGets(typeof(EnumWarpWeft));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit", (int)Session[SessionInfo.currentUserID]);            
            ViewBag.Currencys = Currency.Gets("SELECT * FROM Currency", (int)Session[SessionInfo.currentUserID]);
            return View(oWUSubContract);
        }

        [HttpPost]
        public JsonResult Save(WUSubContract oWUSubContract)
        {
            _oWUSubContract = new WUSubContract();
            try
            {
                _oWUSubContract = oWUSubContract;
                _oWUSubContract = _oWUSubContract.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContract = new WUSubContract();
                _oWUSubContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(WUSubContract oWUSubContract)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oWUSubContract.Delete(oWUSubContract.WUSubContractID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(WUSubContract oWUSubContract)
        {
            _oWUSubContract = new WUSubContract();
            try
            {
                _oWUSubContract = oWUSubContract;
                _oWUSubContract = _oWUSubContract.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContract = new WUSubContract();
                _oWUSubContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FinishYarnChallan(WUSubContract oWUSubContract)
        {
            _oWUSubContract = new WUSubContract();
            try
            {
                _oWUSubContract = oWUSubContract;
                _oWUSubContract = _oWUSubContract.FinishYarnChallan((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContract = new WUSubContract();
                _oWUSubContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptRevise(WUSubContract oWUSubContract)
        {
            _oWUSubContract = new WUSubContract();
            try
            {
                _oWUSubContract = oWUSubContract;
                _oWUSubContract = _oWUSubContract.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContract = new WUSubContract();
                _oWUSubContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContract);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Revise History

        [HttpPost]
        public JsonResult ReviseHistory(WUSubContract oWUSubContract)
        {
            List<WUSubContract> oWUSubContracts = new List<WUSubContract>();
            try
            {
                string sSQL = "SELECT * FROM View_WUSubContractLog AS HH WHERE HH.WUSubContractID = " + oWUSubContract.WUSubContractID;
                oWUSubContracts = WUSubContract.Get(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContract = new WUSubContract();
                _oWUSubContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWUSubContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

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

        [HttpPost]
        public JsonResult GetConditions(TermsAndCondition oTermsAndCondition)
        {
            List<TermsAndCondition> oTermsAndConditions = new List<TermsAndCondition>();
            try
            {
                oTermsAndConditions = TermsAndCondition.GetsByModule((int)EnumModuleName.WUSubContract, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                TermsAndCondition oTempTermsAndCondition = new TermsAndCondition();
                oTempTermsAndCondition.ErrorMessage = ex.Message;
                oTermsAndConditions.Add(oTempTermsAndCondition);
            }
            var jSonResult = Json(oTermsAndConditions, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #region Print PDF & Excel

        [HttpPost]
        public ActionResult SetSessionData(WUSubContract oWUSubContract)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oWUSubContract);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPreview(int id, double ts)
        {
            WUSubContract oWUSubContract = new WUSubContract();            
            try
            {
                oWUSubContract = oWUSubContract.Get(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract.WUSubContractYarnConsumptions = WUSubContractYarnConsumption.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract.WUSubContractTermsConditions = WUSubContractTermsCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWUSubContract = new WUSubContract();
                oWUSubContract.ErrorMessage = ex.Message;
            }

            this.Session.Remove(SessionInfo.ParamObj);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            
            rptWUSubContract rptWUSubContract = new rptWUSubContract();
            byte[] abytes = rptWUSubContract.PrepareReport(oWUSubContract, oCompany);
            return File(abytes, "application/pdf");

        }

        public ActionResult RevisedPrintPreview(int id, double ts)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            try
            {
                oWUSubContract = oWUSubContract.GetRevise(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract.WUSubContractYarnConsumptions = WUSubContractYarnConsumption.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract.WUSubContractTermsConditions = WUSubContractTermsCondition.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWUSubContract = new WUSubContract();
                oWUSubContract.ErrorMessage = ex.Message;
            }

            this.Session.Remove(SessionInfo.ParamObj);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptWUSubContract rptWUSubContract = new rptWUSubContract();
            byte[] abytes = rptWUSubContract.PrepareReport(oWUSubContract, oCompany);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintList(double ts)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            List<WUSubContract> oWUSubContracts = new List<WUSubContract>();
            try
            {
                oWUSubContract = (WUSubContract)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_WUSubContract AS HH WHERE HH.WUSubContractID IN (" + oWUSubContract.ErrorMessage + ") ORDER BY HH.WUSubContractID ASC";
                oWUSubContracts = WUSubContract.Get(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception)
            {
                oWUSubContract = new WUSubContract();
                throw new Exception(oWUSubContract.ErrorMessage);
            }


            this.Session.Remove(SessionInfo.ParamObj);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptWUSubContractList rptWUSubContract = new rptWUSubContractList();
            byte[] abytes = rptWUSubContract.PrepareReport(oWUSubContracts, oCompany);
            return File(abytes, "application/pdf");

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

        public void PrintExcel(double ts)
        {
            Company oCompany = new Company();
            List<WUSubContract> oWUSubContracts = new List<WUSubContract>();
            WUSubContract oWUSubContract = new WUSubContract();

            try
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oWUSubContract = (WUSubContract)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_WUSubContract AS HH WHERE HH.WUSubContractID IN (" + oWUSubContract.ErrorMessage + ") ORDER BY HH.WUSubContractID ASC";
                oWUSubContracts = WUSubContract.Get(sSQL, (int)Session[SessionInfo.currentUserID]);

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

        #endregion

        #region Advance Search

        [HttpPost]
        public JsonResult AdvanceSearch(WUSubContract oWUSubContract)
        {
            List<WUSubContract> oWUSubContracts = new List<WUSubContract>();
            try
            {
                string sSQL = this.GetSQL(oWUSubContract);
                oWUSubContracts = WUSubContract.Get(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContract = new WUSubContract();
                _oWUSubContract.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWUSubContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
                sReturn = sReturn + " HH.WUSubContractID IN ( SELECT WU.WUSubContractID FROM View_WUSubContractYarnConsumption AS WU WHERE WU.YarnName like '%"+ YarnName + "%')";
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
                sReturn = sReturn + " HH.Rate BETWEEN " + txtRateStartPerMeasurementUnit + " AND "+ txtRateEndPerMeasurementUnit + "";
            }
            #endregion

            #region Warp Weft Type
            if ((EnumWarpWeft)nWarpWeftTypeInt !=  EnumWarpWeft.None)
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

            sReturn = sReturn1 + sReturn + " ORDER BY HH.WUSubContractID ASC";
            return sReturn;
        }

        #endregion
    }
}