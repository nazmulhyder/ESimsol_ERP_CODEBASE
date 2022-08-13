using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class AdjustmentRequisitionSlipController : Controller
    {
        #region Declaration
        AdjustmentRequisitionSlip _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
        AdjustmentRequisitionSlipDetail _oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
        List<AdjustmentRequisitionSlip> _oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
        List<AdjustmentRequisitionSlipDetail> _oAdjustmentRequisitionSlipDetails = new List<AdjustmentRequisitionSlipDetail>();
        string _sErrorMessage = "";
        #endregion

        #region Action
        public ActionResult ViewAdjustmentRequisitionSlips(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
            _oAdjustmentRequisitionSlips = AdjustmentRequisitionSlip.Gets("Select * from VIEW_AdjustmentRequisitionSlip where  isnull(AprovedByID,0)=0 and buid="+buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.InOutTypes = EnumObject.jGets(typeof(EnumInOutType));
            ViewBag.BUID = buid;
            ViewBag.ProductCategories = ProductCategory.GetsBUWiseLastLayer(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.AdjustmentTypes = EnumObject.jGets(typeof(EnumAdjustmentType));
            return View(_oAdjustmentRequisitionSlips);
        }
        public ActionResult ViewAdjustmentRequisitionSlip(int buid,  int id, double ts)
        {
            _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
           
            if (id > 0)
            {
                _oAdjustmentRequisitionSlip = AdjustmentRequisitionSlip.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID > 0)
                {
                    _oAdjustmentRequisitionSlip.ARSDetails = AdjustmentRequisitionSlipDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            ViewBag.WorkingUnits = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.Adjustment, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.InOutTypes = EnumObject.jGets(typeof(EnumInOutType));
            ViewBag.AdjustmentTypes = EnumObject.jGets(typeof(EnumAdjustmentType));
            
            return View(_oAdjustmentRequisitionSlip);
        }

        private bool ValidateInput(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            if (oAdjustmentRequisitionSlip.WorkingUnitID <= 0)
            {
                _sErrorMessage = "Please pick Store";
                return false;
            }
            return true;
        }

        [HttpPost]
        public JsonResult Save(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            try
            {
                //if (this.ValidateInput(oAdjustmentRequisitionSlip))
                //{
                    _oAdjustmentRequisitionSlip = oAdjustmentRequisitionSlip.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID > 0)
                    {
                        _oAdjustmentRequisitionSlip.ARSDetails = AdjustmentRequisitionSlipDetail.Gets(_oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                //}
                //else
                //{
                //    _oAdjustmentRequisitionSlip.ErrorMessage = _sErrorMessage;
                //}

            }
            catch (Exception ex)
            {
                _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                _oAdjustmentRequisitionSlip.ErrorMessage = "Invalid entry";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAdjustmentRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Approve(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            string sErrorMease = "";
            _oAdjustmentRequisitionSlip = oAdjustmentRequisitionSlip;
            try
            {
                _oAdjustmentRequisitionSlip = oAdjustmentRequisitionSlip.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAdjustmentRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            try
            {
                if (oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID <= 0) { throw new Exception("Please select an valid item."); }
                oAdjustmentRequisitionSlip.ErrorMessage = oAdjustmentRequisitionSlip.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                oAdjustmentRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAdjustmentRequisitionSlip.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail(AdjustmentRequisitionSlipDetail oAdjustmentRequisitionSlipDetail)
        {
            try
            {
                if (oAdjustmentRequisitionSlipDetail.AdjustmentRequisitionSlipDetailID > 0)
                {
                    oAdjustmentRequisitionSlipDetail.ErrorMessage = oAdjustmentRequisitionSlipDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oAdjustmentRequisitionSlipDetail = new AdjustmentRequisitionSlipDetail();
                oAdjustmentRequisitionSlipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAdjustmentRequisitionSlipDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Get Products
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();

            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProduct.ProductName = oProduct.ProductName.Trim();
                    oProducts = Product.Gets("SELECT * FROM View_Product WHERE Activity=1 and ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID = " + oProduct.BUID + ") AND ProductName Like '%" + oProduct.ProductName + "%' ", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.Adjustment, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.Gets("SELECT * FROM View_Product WHERE Activity=1 and ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID = " + oProduct.BUID + ") ", ((User)Session[SessionInfo.CurrentUser]).UserID);
                   // oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.Adjustment, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                string sLotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";
                int nProductID = oLot.ProductID;
                int nWorkingUnitID = oLot.WorkingUnitID;
                int nBUID = oLot.BUID;

                string sSQL = "Select * from View_Lot Where LotID<>0 ";
                if (nBUID>0)
                {
                    sSQL = sSQL + " And BUID = " + nBUID.ToString();
                }
                if (!string.IsNullOrEmpty(sLotNo))
                {
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                }
                else
                {
                    sSQL = sSQL + " And ROUND(Balance,2) > 0";
                }

                if (nProductID > 0)//Requirement of Shohel Rana of B007
                    sSQL = sSQL + " And ProductID=" + nProductID;
                if (nWorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + nWorkingUnitID;
                if (oLot.StyleID > 0)
                    sSQL = sSQL + " And StyleID=" + oLot.StyleID;
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            _oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
            try
            {
                string sSQL = MakeSQL(oAdjustmentRequisitionSlip);
                _oAdjustmentRequisitionSlips = AdjustmentRequisitionSlip.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
                _oAdjustmentRequisitionSlip.ErrorMessage = ex.Message;
                _oAdjustmentRequisitionSlips.Add(_oAdjustmentRequisitionSlip);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAdjustmentRequisitionSlips);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            string sParams = oAdjustmentRequisitionSlip.Note;

            int nCboDate = 0;
            DateTime dFromDate = DateTime.Today;
            DateTime dToDate = DateTime.Today;
            int nAdjustmentTypeType = 0;
            string sProductIDs = "";
            string sARSlipNo = "";
            int nBUID = 0;
            int nProductCategory = 0;
            int nAdjustmentType = 0;


            if (!string.IsNullOrEmpty(sParams))
            {
                string sTemp = "";
                sProductIDs = Convert.ToString(sParams.Split('~')[0]);
                nCboDate = Convert.ToInt32(sParams.Split('~')[1]);
                dFromDate = Convert.ToDateTime(sParams.Split('~')[2]);
                dToDate = Convert.ToDateTime(sParams.Split('~')[3]);
                nAdjustmentTypeType = Convert.ToInt32(sParams.Split('~')[4]);
                sARSlipNo = Convert.ToString(sParams.Split('~')[5]);
                nBUID = Convert.ToInt32(sParams.Split('~')[6]);
                nProductCategory = Convert.ToInt32(sParams.Split('~')[7]);
                nAdjustmentType = Convert.ToInt32(sParams.Split('~')[8]);
            }

            string sReturn1 = "SELECT * FROM View_AdjustmentRequisitionSlip AS ARS ";
            string sReturn = "";

            #region Date
            if (nCboDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ARS.[Date],106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ARS.[Date],106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ARS.[Date],106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ARS.[Date],106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ARS.[Date],106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ARS.[Date],106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "AdjustmentRequisitionSlipID in (Select ARS.AdjustmentRequisitionSlipID from VIEW_AdjustmentRequisitionSlipDetail as ARS where ProductID in (" + sProductIDs + "))";
            }
            #endregion

            #region ARSlipNo
            if (!string.IsNullOrEmpty(sARSlipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ARS.ARSlipNo Like  '%" + sARSlipNo + "%'";
            }
            #endregion

            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ARS.BUID = " + nBUID;
            }
            #endregion

            #region Product Category
            if (nProductCategory > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "AdjustmentRequisitionSlipID in (Select ARS.AdjustmentRequisitionSlipID from VIEW_AdjustmentRequisitionSlipDetail as ARS where ProductCategoryID =" + nProductCategory+ ")";
            }
            #endregion
            #region Adjustment Type
            if (nAdjustmentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AdjustmentType =" + nAdjustmentType;
            }
            #endregion


            string sSQL = sReturn1 + " " + sReturn ;
            return sSQL;
        }
    
        #endregion

        #region GetCompanyLogo
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
        #endregion

        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            try
            {
                oAdjustmentRequisitionSlip = oAdjustmentRequisitionSlip.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                oAdjustmentRequisitionSlip.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAdjustmentRequisitionSlip);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        [HttpPost]
        public ActionResult SetAdjustmentRequisitionSlipData(AdjustmentRequisitionSlip oAdjustmentRequisitionSlip)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oAdjustmentRequisitionSlip);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExcelAdjustmentRequisitionSlipList()
        {
            _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
            try
            {
                _oAdjustmentRequisitionSlip = (AdjustmentRequisitionSlip)Session[SessionInfo.ParamObj];
                _oAdjustmentRequisitionSlips = AdjustmentRequisitionSlip.Gets("SELECT * FROM View_AdjustmentRequisitionSlip WHERE AdjustmentRequisitionSlipID IN (" + _oAdjustmentRequisitionSlip.ErrorMessage + ") Order By AdjustmentRequisitionSlipID", (int)Session[SessionInfo.currentUserID]);
                _oAdjustmentRequisitionSlipDetails = AdjustmentRequisitionSlipDetail.Gets("SELECT * FROM View_AdjustmentRequisitionSlipDetail WHERE AdjustmentRequisitionSlipID IN (" + _oAdjustmentRequisitionSlip.ErrorMessage + ") Order By AdjustmentRequisitionSlipID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                _oAdjustmentRequisitionSlips = new List<AdjustmentRequisitionSlip>();
            }

            if (_oAdjustmentRequisitionSlips.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oAdjustmentRequisitionSlip.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oAdjustmentRequisitionSlip.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                //bool bIsRateView = false;
                //List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
                //oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

                //oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
                //if (oAuthorizationRoleMapping.Count > 0)
                //{
                //    bIsRateView = true;
                //}

                int count = 0, nStartCol = 2, nTotalCol = 0;

                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Adjustment Requisition Slip Details");
                    sheet.Name = "Adjustment Requisition Slip Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 15; //Req No
                    sheet.Column(nStartCol++).Width = 12; //Date
                    sheet.Column(nStartCol++).Width = 25; //Prepare By
                    sheet.Column(nStartCol++).Width = 15; //Approve BY
                    sheet.Column(nStartCol++).Width = 25; //WUName
                    sheet.Column(nStartCol++).Width = 30; //Product Name
                    sheet.Column(nStartCol++).Width = 18; //Qty
                    sheet.Column(nStartCol++).Width = 15; //LotNo
                    sheet.Column(nStartCol++).Width = 15; //In OutType
                    sheet.Column(nStartCol++).Width = 15; //Adjustment Type
                    //if (bIsRateView)
                    //{
                    //    sheet.Column(nStartCol++).Width = 12; //U. Price
                    //    sheet.Column(nStartCol++).Width = 20; //Note
                    //}


                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Adjustment Requisition Slip Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Req No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Preapare By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Approve By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Store Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "In Out Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (AdjustmentRequisitionSlip oItem in _oAdjustmentRequisitionSlips)
                    {
                        List<AdjustmentRequisitionSlipDetail> oTempAdjustmentRequisitionSlipDetails = new List<AdjustmentRequisitionSlipDetail>();
                        oTempAdjustmentRequisitionSlipDetails = _oAdjustmentRequisitionSlipDetails.Where(x => x.AdjustmentRequisitionSlipID == oItem.AdjustmentRequisitionSlipID).ToList();
                        int rowCount = (oTempAdjustmentRequisitionSlipDetails.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ARSlipNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.DateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.PreaperByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.AprovedByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.WUName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        int colindex = nStartCol;
                        #endregion

                        #region Detail
                        if (oTempAdjustmentRequisitionSlipDetails.Count > 0)
                        {
                            foreach (AdjustmentRequisitionSlipDetail oItemDetail in oTempAdjustmentRequisitionSlipDetails)
                            {
                                nStartCol = 8;
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Qty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.LotNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;                                     
                                rowIndex++;
                            }
                        }
                        else
                        {
                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                      
                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = _oAdjustmentRequisitionSlipDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

           
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Adjustment_Requisition_Slip_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
                #endregion

        }
        public ActionResult PrintAdjustmentRequisitionSlipList()
        {
            _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
            try
            {
                _oAdjustmentRequisitionSlip = (AdjustmentRequisitionSlip)Session[SessionInfo.ParamObj];
                _oAdjustmentRequisitionSlips = AdjustmentRequisitionSlip.Gets("SELECT * FROM View_AdjustmentRequisitionSlip WHERE AdjustmentRequisitionSlipID IN (" + _oAdjustmentRequisitionSlip.ErrorMessage + ") Order By AdjustmentRequisitionSlipID", (int)Session[SessionInfo.currentUserID]);
                _oAdjustmentRequisitionSlipDetails = AdjustmentRequisitionSlipDetail.Gets("SELECT * FROM View_AdjustmentRequisitionSlipDetail WHERE AdjustmentRequisitionSlipID IN (" + _oAdjustmentRequisitionSlip.ErrorMessage + ") Order By AdjustmentRequisitionSlipID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
                _oAdjustmentRequisitionSlipDetails = new List<AdjustmentRequisitionSlipDetail>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oAdjustmentRequisitionSlip.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oAdjustmentRequisitionSlip.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            //bool bIsRateView = false;
            //List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            //oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DUReturnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            //oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            //if (oAuthorizationRoleMapping.Count > 0)
            //{
            //    bIsRateView = true;
            //}

            rptAdjustmentRequisitionSlips oReport = new rptAdjustmentRequisitionSlips();
            byte[] abytes = oReport.PrepareReport(_oAdjustmentRequisitionSlips, _oAdjustmentRequisitionSlipDetails, oCompany);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintAdjustmentRequisitionSlip(int id)
        {
            _oAdjustmentRequisitionSlip = new AdjustmentRequisitionSlip();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oAdjustmentRequisitionSlip = AdjustmentRequisitionSlip.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oAdjustmentRequisitionSlipDetails = AdjustmentRequisitionSlipDetail.Gets(_oAdjustmentRequisitionSlip.AdjustmentRequisitionSlipID, (int)Session[SessionInfo.currentUserID]);
                //_oAdjustmentRequisitionSlip.BusinessUnit = oBusinessUnit.Get(_oAdjustmentRequisitionSlip.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                //_oAdjustmentRequisitionSlip.Company = oCompany;

                List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
                oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.AdjustmentRequisitionSlipPreview, (int)Session[SessionInfo.currentUserID]);

                byte[] abytes;
                rptAdjustmentRequisitionSlip oReport = new rptAdjustmentRequisitionSlip();
                abytes = oReport.PrepareReport(_oAdjustmentRequisitionSlip, _oAdjustmentRequisitionSlipDetails, oCompany, oSignatureSetups);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }

        }
        #endregion

    }
}