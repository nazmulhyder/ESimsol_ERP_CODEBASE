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
namespace ESimSolFinancial.Controllers
{
    public class ProductionSheetController : Controller
    {

        #region Declaration

        ProductionSheet _oProductionSheet = new ProductionSheet();
        List<ProductionSheet> _oProductionSheets = new List<ProductionSheet>();
        ProductionRecipe _oProductionRecipe = new ProductionRecipe();
        List<ProductionRecipe> _oProductionRecipes = new List<ProductionRecipe>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        ReviseRequest _oReviseRequest = new ReviseRequest();
        Product _oProduct = new Product();
        List<Product> _oProducts = new List<Product>();
        List<ProductionProcedure> _oProductionProcedures = new List<ProductionProcedure>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        PTUUnit2 _oPTUUnit2 = new PTUUnit2();
        RMRequisition _oRMRequisition = new RMRequisition();
        #endregion

        #region Functions

        #endregion

        #region Actions
        public ActionResult ViewProductionSheets(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oProductionSheets = new List<ProductionSheet>();
            string sSQL = "SELECT * FROM View_ProductionSheet WHERE BUID = "+buid+" AND ProductNature = "+ProductNature+" AND SheetStatus = "+(int)EnumProductionSheetStatus.Initialize;
            _oProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            return View(_oProductionSheets);
        }
        public ActionResult ViewProductionSheet(int id, int PTUUnit2ID)
        {
            _oProductionSheet = new ProductionSheet();
            if (id > 0)
            {
                _oProductionSheet = _oProductionSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionRecipes = ProductionRecipe.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else if (PTUUnit2ID>0)
            {
                _oPTUUnit2 = new PTUUnit2();
                _oPTUUnit2 = _oPTUUnit2.Get(PTUUnit2ID, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.PTUUnit2ID = _oPTUUnit2.PTUUnit2ID;
                _oProductionSheet.ExportPINo = _oPTUUnit2.ExportPINo;
                _oProductionSheet.ContractorID = _oPTUUnit2.ContractorID;
                _oProductionSheet.BuyerName = _oPTUUnit2.BuyerName;
                _oProductionSheet.ContractorName = _oPTUUnit2.ContractorName;
                _oProductionSheet.ProductID = _oPTUUnit2.ProductID;
                _oProductionSheet.ProductName = _oPTUUnit2.ProductName;
                _oProductionSheet.ProductCode = _oPTUUnit2.ProductCode;
                _oProductionSheet.ProdOrderQty = _oPTUUnit2.ProdOrderQty;
                _oProductionSheet.YetToSheetQty = _oPTUUnit2.YetToProductionSheeteQty;
                _oProductionSheet.ModelReferenceID = _oPTUUnit2.ModelReferenceID;
                _oProductionSheet.ModelReferencenName = _oPTUUnit2.ModelReferenceName;
                _oProductionSheet.FGWeight = _oPTUUnit2.FinishGoodsWeight;
                _oProductionSheet.FGWeightUnitID = _oPTUUnit2.FinishGoodsUnit;
                _oProductionSheet.NaliWeight = _oPTUUnit2.NaliWeight;
                _oProductionSheet.WeightFor = _oPTUUnit2.WeightFor;
                _oProductionSheet.FGWeightUnitSymbol = _oPTUUnit2.FGUSymbol;
                _oProductionSheet.ColorName = _oPTUUnit2.ColorName;
                _oProductionSheet.UnitSymbol = _oPTUUnit2.UnitSymbol;
            }
            return View(_oProductionSheet);
        }
        public ActionResult ViewProductionSheetForPoly(int id, int PTUUnit2ID)
        {
            _oProductionSheet = new ProductionSheet();
            if (id > 0)
            {
                _oProductionSheet = _oProductionSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionRecipes = ProductionRecipe.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else if (PTUUnit2ID > 0)
            {
                _oPTUUnit2 = new PTUUnit2();
                _oPTUUnit2 = _oPTUUnit2.Get(PTUUnit2ID, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.PTUUnit2ID = _oPTUUnit2.PTUUnit2ID;
                _oProductionSheet.ExportPINo = _oPTUUnit2.ExportPINo;
                _oProductionSheet.ContractorID = _oPTUUnit2.ContractorID;
                _oProductionSheet.BuyerName = _oPTUUnit2.BuyerName;
                _oProductionSheet.ContractorName = _oPTUUnit2.ContractorName;
                _oProductionSheet.ProductID = _oPTUUnit2.ProductID;
                _oProductionSheet.ProductName = _oPTUUnit2.ProductName;
                _oProductionSheet.ProductCode = _oPTUUnit2.ProductCode;
                _oProductionSheet.ProdOrderQty = _oPTUUnit2.ProdOrderQty;
                _oProductionSheet.YetToSheetQty = _oPTUUnit2.YetToProductionSheeteQty;
                _oProductionSheet.ModelReferenceID = _oPTUUnit2.ModelReferenceID;
                _oProductionSheet.ModelReferencenName = _oPTUUnit2.ModelReferenceName;
                _oProductionSheet.FGWeight = _oPTUUnit2.FinishGoodsWeight;
                _oProductionSheet.FGWeightUnitID = _oPTUUnit2.FinishGoodsUnit;
                _oProductionSheet.NaliWeight = _oPTUUnit2.NaliWeight;
                _oProductionSheet.FGWeightUnitSymbol = _oPTUUnit2.FGUSymbol;
                _oProductionSheet.Measurement = _oPTUUnit2.Measurement;
                _oProductionSheet.ColorName = _oPTUUnit2.ColorName;
                _oProductionSheet.UnitSymbol = _oPTUUnit2.UnitSymbol;
            }
            ViewBag.FGUnits = MeasurementUnit.Gets(EnumUniteType.Weight,(int)Session[SessionInfo.currentUserID]);
            return View(_oProductionSheet);
        }
        public ActionResult ViewChangeRawMaterial(int id)
        {
            _oProductionSheet = new ProductionSheet();
            if (id > 0)
            {
                _oProductionSheet = _oProductionSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionRecipes = ProductionRecipe.Gets(id, (int)Session[SessionInfo.currentUserID]);

            }
            string sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID IN (" + string.Join(",", _oProductionSheet.ProductionRecipes.Select(x => x.ProductID)) + ") ORDER BY HH.ProductID ASC";
            ViewBag.UnitConversions = UnitConversion.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oProductionSheet);
        }
        public ActionResult ViewChangeRawMaterialForPoly(int id)
        {
            _oProductionSheet = new ProductionSheet();
            if (id > 0)
            {
                _oProductionSheet = _oProductionSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionRecipes = ProductionRecipe.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            string sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID IN (" + string.Join(",", _oProductionSheet.ProductionRecipes.Select(x => x.ProductID)) + ") ORDER BY HH.ProductID ASC";
            ViewBag.UnitConversions = UnitConversion.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oProductionSheet);
        }


        [HttpPost]
        public JsonResult Save(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = oProductionSheet;
                _oProductionSheet.SheetStatus = (EnumProductionSheetStatus)oProductionSheet.SheetStatusInInt;
                _oProductionSheet = _oProductionSheet.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionSheet = new ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeRawMaterial(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = oProductionSheet;
                _oProductionSheet = _oProductionSheet.ChangeRawMaterial((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionSheet = new ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ProductionSheet oProductionSheet = new ProductionSheet();
                sFeedBackMessage = oProductionSheet.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = oProductionSheet;
                _oProductionSheet.SheetStatus = (EnumProductionSheetStatus)oProductionSheet.SheetStatusInInt;
                _oProductionSheet = _oProductionSheet.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionSheet = new ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionStart(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = oProductionSheet;
                _oProductionSheet.SheetStatus = EnumProductionSheetStatus.Production_In_Progress;
                _oProductionSheet = _oProductionSheet.ProductionStart((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionSheet = new ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductionUndo(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = oProductionSheet;
                _oProductionSheet.SheetStatus = EnumProductionSheetStatus.Approved;
                _oProductionSheet = _oProductionSheet.ProductionUndo((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionSheet = new ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApproved(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = oProductionSheet;
                _oProductionSheet.SheetStatus = EnumProductionSheetStatus.Initialize;
                _oProductionSheet = _oProductionSheet.UndoApproved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionSheet = new ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Setup Priduction Procedure
        public ActionResult ViewProductionProcedure(int id)
        {
            _oProductionSheet = new ProductionSheet();
            if (id > 0)
            {
                _oProductionSheet = _oProductionSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionProcedures = ProductionProcedure.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            
            _oProductionSheet.ProductionSteps = ProductionStep.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProductionSheet);
        }

        [HttpPost]
        public JsonResult SaveProductionProcedure(ProductionSheet oProductionSheet)
        {
            _oProductionProcedures = new List<ProductionProcedure>();
            try
            {
                _oProductionProcedures = ProductionProcedure.Save(oProductionSheet, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ProductionProcedure oProductionProcedure = new ProductionProcedure();
                _oProductionProcedures = new List<ProductionProcedure>();
                oProductionProcedure.ErrorMessage = ex.Message;
                _oProductionProcedures.Add(oProductionProcedure);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionProcedures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SEARCHING
        private string GetSQL(string sTemp)
        {
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sPSNo = sTemp.Split('~')[3];
            string sPINo = sTemp.Split('~')[4];
            string sBuyerIDs = sTemp.Split('~')[5];
            string sProductIDs = sTemp.Split('~')[6];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[8]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[9]);
            int nProductNature = Convert.ToInt32(sTemp.Split('~')[10]);
            string sReturn1 = "SELECT * FROM View_ProductionSheet";
            string sReturn = "";

            #region PO No

            if (sPSNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SheetNo LIKE '%" + sPSNo + "%'";
            }
            #endregion

            #region Party PO No

            if (sPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportPINo LIKE '%" + sPINo + "%'";
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Product

            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + sProductIDs + ")";
            }
            #endregion

           

            #region Issue Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate>= '" + dSOStartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate< '" + dSOStartDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion


            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(ApprovedBy,0) = 0";
            }
            #endregion

            #region BU
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

           

            #region Product Nature
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(ProductNature,0) = " + nProductNature;
            #endregion


            sReturn = sReturn1 + sReturn + " ORDER BY ContractorID, ProductionSheetID";
            return sReturn;
        }
        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            try
            {
                string sSQL = GetSQL(Temp);
                oProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionSheets = new List<ProductionSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Product BU, User and Name wise ( write by Mahabub)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ProductionSheet, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ProductionSheet, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get PEReceipe
        [HttpPost]
        public JsonResult GetProductionRecipes(Recipe oRecipe)
        {
            ProductionSheet oProductionSheet = new ProductionSheet();
            List<RecipeDetail> oRecipeDetails = new List<RecipeDetail>();
            List<UnitConversion> oUnitConversions = new List<UnitConversion>();
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();            
            try
            {
                oRecipeDetails = RecipeDetail.Gets(oRecipe.RecipeID, (int)Session[SessionInfo.currentUserID]);
                foreach(RecipeDetail oItem in oRecipeDetails)
                {
                    _oProductionRecipe = new ProductionRecipe();
                    _oProductionRecipe.ProductID = oItem.ProductID;
                    _oProductionRecipe.QtyInPercent = oItem.QtyInPercent;
                    _oProductionRecipe.RequiredQty = 0;
                    _oProductionRecipe.QtyType = oItem.QtyType;
                    _oProductionRecipe.QtyTypeInt = oItem.QtyTypeInt;
                    _oProductionRecipe.Remarks = oItem.Note;
                    _oProductionRecipe.ProductCode = oItem.ProductCode;
                    _oProductionRecipe.ProductName = oItem.ProductName;
                    _oProductionRecipe.MUnitID = oItem.MeasurementUnitID;
                    _oProductionRecipe.MUName = oItem.MUnit;
                    oProductionRecipes.Add(_oProductionRecipe);
                }
                string sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID IN (SELECT MM.ProductID FROM RecipeDetail AS MM WHERE MM.RecipeID=" + oRecipe.RecipeID.ToString() + ") ORDER BY HH.ProductID ASC";
                oUnitConversions = UnitConversion.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);               

                oProductionSheet = new ProductionSheet();
                oProductionSheet.ProductionRecipes= oProductionRecipes;
                oProductionSheet.UnitConversions = oUnitConversions;
            }
            catch (Exception ex)
            {
                oProductionSheet = new ProductionSheet();
                oProductionSheet.ErrorMessage = ex.Message;                
            }
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Production Sheet By Business Unit

        [HttpPost]
        public JsonResult GetsPSForRMO(ProductionSheet oProductionSheet)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            try
            {
                string sSQL = "Select * from View_ProductionSheet Where YetToRMOQty>0 And BUID=" + oProductionSheet.BUID + "";
                if (oProductionSheet.ContractorID > 0)
                {
                    sSQL += " And ContractorID=" + oProductionSheet.ContractorID;
                }
                if (!string.IsNullOrEmpty(oProductionSheet.SheetNo))
                {
                    sSQL += " And SheetNo Like '%" + oProductionSheet.SheetNo.Trim() + "%'";
                }
                oProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionSheets = new List<ProductionSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult GetsProductionSheets(ProductionSheet oProductionSheet)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            try
            {
                string sSQL = "Select * from View_ProductionSheet Where ISNULL(ApprovedBy,0)!=0 And BUID=" + oProductionSheet.BUID;

                if (oProductionSheet.ProductNatureInInt > 0)
                {
                    sSQL += " And ProductNature=" + oProductionSheet.ProductNatureInInt;
                }

                if (oProductionSheet.ExportPINo != null && oProductionSheet.ExportPINo != "")
                {
                    sSQL += " And ExportPINo like '%" + oProductionSheet.ExportPINo + "%'";
                }
                else
                {
                    if (oProductionSheet.ContractorID > 0)
                    {
                        sSQL += " And ContractorID=" + oProductionSheet.ContractorID;
                    }
                    if (oProductionSheet.SheetNo != null && oProductionSheet.SheetNo != "")
                    {
                        sSQL += " And SheetNo Like '%" + oProductionSheet.SheetNo.Trim() + "%'";
                    }
                }
                oProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionSheets = new List<ProductionSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsProductionSheetsForQC(ProductionSheet oProductionSheet)
        {
            List<ProductionSheet> oProductionSheets = new List<ProductionSheet>();
            try
            {
                string sSQL = "Select * from View_ProductionSheet Where ISNULL(ApprovedBy,0)!=0 And BUID=" + oProductionSheet.BUID + " AND ProductNature = " + oProductionSheet.ProductNatureInInt + " AND ProductionSheetID IN ( SELECT PE.ProductionSheetID FROM View_ProductionExecution AS PE WHERE ISNULL(PE.YetToQC,0)>0 )";
                if (oProductionSheet.ExportPINo == null) { oProductionSheet.ExportPINo = ""; }
                if (oProductionSheet.SheetNo == null) { oProductionSheet.SheetNo = ""; }
                if (oProductionSheet.ExportPINo != "")
                {
                    sSQL += " And ExportPINo like '%" + oProductionSheet.ExportPINo + "%'";
                }
                else
                {
                    if (oProductionSheet.ContractorID > 0)
                    {
                        sSQL += " And ContractorID=" + oProductionSheet.ContractorID;
                    }
                    if (oProductionSheet.SheetNo != "")
                    {
                        sSQL += " And SheetNo Like '%" + oProductionSheet.SheetNo.Trim() + "%'";
                    }
                }
                oProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionSheets = new List<ProductionSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        //public ActionResult ProductionSheetPrintList(string sIDs, double ts)
        //{
        //    _oProductionSheet = new ProductionSheet();
        //    _oProductionSheets = new List<ProductionSheet>();
        //    string sSql = "SELECT * FROM View_ProductionSheet WHERE ProductionSheetID IN (" + sIDs + ")";
        //    _oProductionSheets = ProductionSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
        //    int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
        //    _oProductionSheet.ProductionSheetList = _oProductionSheets;
        //    if (_oProductionSheet.ProductionSheetList.Count > 0)
        //    {
        //        Company oCompany = new Company();
        //        oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //        _oProductionSheet.Company = oCompany;
        //        rptProductionSheetList oReport = new rptProductionSheetList();
        //        byte[] abytes = oReport.PrepareReport(_oProductionSheet);
        //        return File(abytes, "application/pdf");
        //    }
        //    else
        //    {

        //        string sMessage = "There is no data for print";
        //        return RedirectToAction("MessageHelper", "User", new { message = sMessage });
        //    }

        //}
        #region Poly Print
        [HttpPost]
        public ActionResult SetProductionSheetListData(ProductionSheet oProductionSheet)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oProductionSheet);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductionSheetPreviewForPoly(int nCriteria, int nBUID)
        {
            _oProductionSheet = new ProductionSheet();
            _oProductionRecipes = new List<ProductionRecipe>();
            _oRMRequisition = new RMRequisition();
            Company oCompany = new Company();
            List<ProductionProcedure> _oProductionProcedures = new List<ProductionProcedure>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oProductionSheet = (ProductionSheet)Session[SessionInfo.ParamObj];
            string sSQL = "";
                if(nCriteria!=5)//Required Raw Material
                {
                    sSQL = "SELECT * FROM View_ProductionProcedure WHERE ProductionSheetID IN (" + _oProductionSheet.Note + ")  AND ProductionStepType = " + nCriteria;
                    _oProductionProcedures = ProductionProcedure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    sSQL = "SELECT * FROM View_ProductionSheet WHERE ProductionSheetID IN (SELECT VPP.ProductionSheetID FROM View_ProductionProcedure AS VPP WHERE VPP.ProductionSheetID IN (" + _oProductionSheet.Note + ")  AND VPP.ProductionStepType = " + nCriteria + ") Order By ProductionSheetID";
                    _oProductionSheets = ProductionSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);    
                }
                _oProductionSheet.BusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionSheets = _oProductionSheets;
                _oProductionSheet.ProductionProcedures = _oProductionProcedures;

                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oProductionSheet.Company = oCompany;

                byte[] abytes;
                if(_oProductionSheets.Count>0)    
                {
                    rptProductionSheetForPoly oReport = new rptProductionSheetForPoly();
                    abytes = oReport.PrepareReport(_oProductionSheet, nCriteria);
                    return File(abytes, "application/pdf");
                }else
                {
                    return RedirectToAction("MessageHelper", "User", new { message = " There is no Sheet for " + ((EnumProductionStepType)nCriteria).ToString() + " Section" });
                }           
 
        }
        #endregion

        public ActionResult ProductionSheetRequiredRawMaterial(int nBUID)//for plastic
        {
            _oProductionSheet = new ProductionSheet();
            _oProductionRecipes = new List<ProductionRecipe>();
            Company oCompany = new Company();
            List<ProductionProcedure> _oProductionProcedures = new List<ProductionProcedure>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oProductionSheet = (ProductionSheet)Session[SessionInfo.ParamObj];//don't delete come from 
            string sSQL = "";
            if (nBUID > 0)
            {
                sSQL = "SELECT * FROM View_RMRequisitionMaterial WHERE ProductionSheetID IN (" + _oProductionSheet.Note + ") Order By ProductionSheetID, ProductID";
                _oRMRequisition.RMRequisitionMaterials = RMRequisitionMaterial.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            _oRMRequisition.BusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oRMRequisition.Company = oCompany;

            byte[] abytes;
            _oProductionSheet.ErrorMessage = (string)Session[SessionInfo.currentUserName];
            rptProductionSheetRequiredRawMaterial oReport = new rptProductionSheetRequiredRawMaterial();
            abytes = oReport.PrepareReport(_oRMRequisition);
            return File(abytes, "application/pdf");
        }

        public ActionResult ProductionSheetPreview(int id)
        {
            _oProductionSheet = new ProductionSheet();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<PETransaction> oPETransactions = new List<PETransaction>();
            if (id > 0)
            {
                _oProductionSheet = _oProductionSheet.Get(id, (int)Session[SessionInfo.currentUserID]);                
                _oProductionSheet.BusinessUnit = oBusinessUnit.Get(_oProductionSheet.BUID, (int)Session[SessionInfo.currentUserID]);                
                _oProductionSheet.ProductionRecipes = ProductionRecipe.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionSheet.ProductionExecutions = ProductionExecution.Gets(_oProductionSheet.ProductionSheetID, (int)Session[SessionInfo.currentUserID]);
                
                List<QC> oQCs = new List<QC>();
                oQCs = QC.Gets(_oProductionSheet.ProductionSheetID, (int)Session[SessionInfo.currentUserID]);
                ProductionExecution oProductionExecution = new ProductionExecution();
                oProductionExecution.StepName = "Actual Finish";
                oProductionExecution.StepShortName = "Actual Finish";
                oProductionExecution.ProductionStepType = EnumProductionStepType.Regular;
                oProductionExecution.ProductionStepTypeInInt = (int)EnumProductionStepType.Regular;
                oProductionExecution.ExecutionQty = oQCs.Sum(oQC => oQC.PassQuantity);
                _oProductionSheet.ProductionExecutions.Add(oProductionExecution);

                string sSQL = "SELECT * FROM View_PETransaction AS HH WHERE HH.ProductionSheetID = " + id.ToString() + " ORDER BY HH.TransactionDate ASC";
                oPETransactions = PETransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oProductionSheet.Company = oCompany;

            byte[] abytes;
            rptProductionSheet oReport = new rptProductionSheet();
            abytes = oReport.PrepareReport(_oProductionSheet, oPETransactions);
            return File(abytes, "application/pdf");
        }

        #endregion Print

        [HttpPost]
        public JsonResult GetProductionSheet(ProductionSheet oProductionSheet)
        {
            _oProductionSheet = new ProductionSheet();
            try
            {
                _oProductionSheet = _oProductionSheet.Get(oProductionSheet.ProductionSheetID,  ((User)Session[SessionInfo.CurrentUser]).UserID);  
            }
            catch (Exception ex)
            {
                _oProductionSheet = new  ProductionSheet();
                _oProductionSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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