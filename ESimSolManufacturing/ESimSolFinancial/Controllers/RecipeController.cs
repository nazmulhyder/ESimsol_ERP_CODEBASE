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
    public class RecipeController : Controller
    { 
        
        #region Declaration
        Recipe _oRecipe = new Recipe();
        List<Recipe> _oRecipes = new List<Recipe>();
        RecipeDetail _oRecipeDetail = new RecipeDetail();
        List<RecipeDetail> _oRecipeDetails = new List<RecipeDetail>();
        CapitalResource _oCapitalResource = new CapitalResource();
        List<Product> _oProducts = new List<Product>();
        List<UnitConversion> _oUnitConversions = new List<UnitConversion>();
        #endregion
        #region Function
        
        #endregion
        public ActionResult ViewRecipes(int buid,int ProductNature, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oRecipes = new List<Recipe>();
            _oRecipes = Recipe.GetsByBUWithProductNature(buid, ProductNature, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            return View(_oRecipes);
        }

        public ActionResult ViewRecipe(int id)
        {
            Product oProduct = new Product();
            _oRecipe = new Recipe();
            _oCapitalResource = new CapitalResource();
            if (id > 0)
            {
                _oRecipe = _oRecipe.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oRecipe.RecipeDetails = RecipeDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oRecipe.RecipeCode = Recipe.GetRecipeNo((int)Session[SessionInfo.currentUserID]);
                _oRecipe.RecipeDetails = new List<RecipeDetail>();
            }
            ViewBag.RecipeTypes = EnumObject.jGets(typeof(EnumRecipeType));
            return View(_oRecipe);
        }

        [HttpGet]
        public JsonResult GetsMUs()
        {
            //EnumProductType{None=0,Rawmaterial = 1,FinishGoods = 2}
            List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
            try
            {
                string sSQL = "SELECT * FROM MeasurementUnit ORDER BY UnitType ASC";
                oMeasurementUnits = MeasurementUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oMeasurementUnits = new List<MeasurementUnit>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMeasurementUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
  
        [HttpPost]
        public JsonResult Save(Recipe oRecipe)
        {   
            _oRecipe = new Recipe();
            try
            {
                _oRecipe= oRecipe;
                _oRecipe = _oRecipe.Save((int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                _oRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult Update(RecipeDetail oRecipe)
        {
            RecipeDetail _oRecipe = new RecipeDetail();
            _oRecipe = oRecipe;
            _oRecipe = _oRecipe.Save((int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {

            int nFinishGoodsID = Convert.ToInt32(sTemp.Split('~')[0]);


            string sReturn1 = "SELECT * FROM View_Recipe";
            string sReturn = "";

            #region Finish Goods
            if (nFinishGoodsID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID = '" + nFinishGoodsID.ToString() + "'";
            }
            #endregion


            sReturn = sReturn1 + sReturn + " ORDER BY RacipeName";
            return sReturn;
        }

        [HttpPost]
        public JsonResult ActiveInActive(Recipe oRecipe)
        {
            _oRecipe = new Recipe();
            try
            {

                _oRecipe = oRecipe.ActiveInActive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<Recipe> oRecipes = new List<Recipe>();
            try
            {
                string sSQL = GetSQL(Temp);
                oRecipes = Recipe.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRecipes = new List<Recipe>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetsRecipeDetails(int id)
        {
            List<RecipeDetail> oRecipeDetails = new List<RecipeDetail>();            
            try
            {
                oRecipeDetails = RecipeDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oRecipeDetails = new List<RecipeDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecipeDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                Recipe oRecipe = new Recipe();
                sErrorMease = oRecipe.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Searching
        public ActionResult RecipeSearch()
        {
            List<Recipe> oRecipes = new List<Recipe>();
            //oRecipes = Recipe.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(oRecipes);
        }
        #endregion

        #region Reports
        public ActionResult PrintRecipes(string sIDs)
        {
            _oRecipe = new Recipe();
            string sSql = "SELECT * FROM View_Recipe WHERE RecipeID IN (" + sIDs + ")";
            _oRecipe.Recipes = Recipe.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            if (_oRecipe.Recipes.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oRecipe.Company = oCompany;
                rptRecipeList oReport = new rptRecipeList();
                byte[] abytes = oReport.PrepareReport(_oRecipe);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }
        public ActionResult PrintRecipeDetails(int id)
        {
            _oRecipe = new Recipe();
            _oRecipe = _oRecipe.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oRecipe.RecipeDetails = RecipeDetail.Gets(_oRecipe.RecipeID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            _oRecipe.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oRecipe.Company = oCompany;

            byte[] abytes;
            rptRecipePreview oReport = new rptRecipePreview();
            abytes = oReport.PrepareReport(_oRecipe);
            return File(abytes, "application/pdf");
        }
        #endregion

        
        [HttpPost]
        public JsonResult GetsRecipeByModelName(Recipe oRecipe)
        {
            _oRecipes = new List<Recipe>();
            _oRecipeDetails = new List<RecipeDetail>();
            try
            {
                string sSQL = "SELECT * FROM Recipe WHERE BUID = " + oRecipe.BUID + " AND ProductNature = " + oRecipe.ProductNatureInInt + "AND RecipeType = " +oRecipe.RecipeType + "  AND RecipeName LIKE '%" + oRecipe.RecipeName + "%'";
                _oRecipes = Recipe.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //sSQL = "SELECT * FROM View_RecipeDetail WHERE RecipeID IN (SELECT RecipeID FROM Recipe WHERE BUID = " + oRecipe.BUID + "  AND ProductNature = " + oRecipe.ProductNatureInInt + "  AND RecipeName LIKE '%" + oRecipe.RecipeName + "%' )";
                //_oRecipeDetails = RecipeDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                //foreach(Recipe oItem in _oRecipes)
                //{
                //    oItem.RecipeDetails = _oRecipeDetails.Where(x => x.RecipeID == oItem.RecipeID).ToList(); 
                //}
            }
            catch (Exception ex)
            {
                _oRecipe = new Recipe();
                _oRecipe.ErrorMessage = ex.Message;
                _oRecipes.Add(_oRecipe);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsProductionRecipeList(Recipe oRecipe)
        {
            _oRecipeDetails = new List<RecipeDetail>();
            ProductionRecipe oProductionRecipe = new ProductionRecipe();
            List<ProductionRecipe> oProductionRecipes = new List<ProductionRecipe>();
            List<Lot> oLots = new List<Lot>();
            List<ProductionRecipe> oTemProductionRecipes = new List<ProductionRecipe>();
            _oUnitConversions = new List<UnitConversion>();
            try
            {                
                _oRecipeDetails = RecipeDetail.Gets(oRecipe.RecipeID, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_Lot WHERE ProductBaseID IN (" + string.Join(",", _oRecipeDetails.Select(x => x.ProductBaseID)) + ") AND BUID = " + oRecipe.BUID + " AND  ParentType IN (" + (int)EnumTriggerParentsType.AdjustmentDetail + "," + (int)EnumTriggerParentsType.GRNDetailDetail + "," + (int)EnumTriggerParentsType.TransferRequisitionDetail+")";
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
               
                int nCount = 0;
                foreach (RecipeDetail oItem in _oRecipeDetails)
                {
                    oProductionRecipe = new ProductionRecipe();
                    oProductionRecipe.ProductionSheetID = 0;
                    oProductionRecipe.ProductID = oItem.ProductID;
                    oProductionRecipe.ProductBaseID = oItem.ProductBaseID;
                    oProductionRecipe.QtyInPercent = oItem.QtyInPercent;
                    oProductionRecipe.MUnitID = oItem.MeasurementUnitID;
                    oProductionRecipe.RequiredQty = 0;
                    oProductionRecipe.StockUnitName = string.IsNullOrEmpty(oLots.Where(x => x.ProductID == oItem.ProductID).Select(x => x.MUName).FirstOrDefault()) ? "" : oLots.Where(x => x.ProductID == oItem.ProductID).Select(x => x.MUName).FirstOrDefault(); 
                    oProductionRecipe.StockBalance = oLots.Where(x =>x.ProductID == oItem.ProductID).Sum(x=>x.Balance);
                    oProductionRecipe.ProductCode = oItem.ProductCode;
                    oProductionRecipe.ProductName = oItem.ProductName;
                    oProductionRecipe.MUName = oItem.MUnit;
                    oProductionRecipe.QtyType = oItem.QtyType;
                    oProductionRecipe.QtyTypeInt = oItem.QtyTypeInt;
                    oProductionRecipe.bIsColor = (nCount%2==0)?true:false;
                    oProductionRecipe.bIsChecked = true;
                    oProductionRecipes.Add(oProductionRecipe);

                    oTemProductionRecipes = oLots.Where(p =>!oProductionRecipes.Any(y => y.ProductID == p.ProductID)).ToList().Where(x => x.ProductBaseID == oItem.ProductBaseID && x.ProductID != oItem.ProductID).GroupBy(item => item.ProductID).Select(group => new ProductionRecipe
                    {
                        ProductID = group.First().ProductID,
                        ProductBaseID = group.First().ProductBaseID,
                        ProductName = group.First().ProductName,
                        ProductCode = group.First().ProductCode,
                        QtyInPercent = oItem.QtyInPercent,
                        MUnitID = oItem.MeasurementUnitID,
                        RequiredQty = 0,
                        MUName = oItem.MUnit,
                        QtyType = oItem.QtyType,
                        QtyTypeInt = oItem.QtyTypeInt,
                        bIsColor = (nCount%2==0)?true:false,
                        StockUnitName = group.First().MUName,
                        StockBalance = group.Sum(x => x.Balance)
                    }).ToList();
                    if (oTemProductionRecipes.Count() > 0)
                    {
                        oProductionRecipes.AddRange(oTemProductionRecipes);
                    }
                    nCount++;
                }
                oProductionRecipes.RemoveAll(x => x.StockBalance <= 0);
                if (oProductionRecipes.Count > 0)
                {
                    sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID IN (" + string.Join(",", oProductionRecipes.Select(x => x.ProductID)) + ") ORDER BY HH.ProductID ASC";
                    _oUnitConversions = UnitConversion.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                if (oRecipe.ProductNatureInInt == (int)EnumProductNature.Hanger)
                {
                    oRecipe.ProductionRecipes = CalculateRequiredQty(oProductionRecipes, oRecipe);
                }
                else if (oRecipe.ProductNatureInInt == (int)EnumProductNature.Poly)
                {
                    oRecipe.ProductionRecipes = CalculateRequiredQtyForPoly(oProductionRecipes, oRecipe);
                }
                oRecipe.UnitConversions = _oUnitConversions;
            }
            catch (Exception ex)
            {
                oProductionRecipe = new ProductionRecipe();
                oProductionRecipe.ErrorMessage = ex.Message;
                oProductionRecipes.Add(oProductionRecipe);
                oRecipe.ProductionRecipes = oProductionRecipes;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


     public List<ProductionRecipe>  CalculateRequiredQty(List<ProductionRecipe> oTempProductionRecipes, Recipe oRecipe)
    {
        
        double nTotalWeight =  Convert.ToDouble(oRecipe.Note.Split('~')[0]);
        double nWeightFor = Convert.ToDouble(oRecipe.Note.Split('~')[1]);
        int nFGMUnitID = Convert.ToInt32(oRecipe.Note.Split('~')[2]);
        double nFGQty = Convert.ToDouble(oRecipe.Note.Split('~')[3]);
        double nPerHangerWeight = (nTotalWeight / nWeightFor);


        foreach (ProductionRecipe oItem in oTempProductionRecipes)
        {
            if (oItem.QtyTypeInt == 1)
            {
                double nConvertedValue = 1;
                double nPerHangerReqQty = (nPerHangerWeight * Convert.ToDouble(oItem.QtyInPercent / 100));
                if (oItem.MUnitID!= nFGMUnitID)
                {
                    nConvertedValue = _oUnitConversions.Where(x => x.ProductID == oItem.ProductID && x.MeasurementUnitID == nFGMUnitID && x.ConvertedUnitID == oItem.MUnitID).Select(x => x.ConvertedUnitValue).FirstOrDefault(); 
                }
                if (nConvertedValue <= 0)
                {
                    oTempProductionRecipes = new List<ProductionRecipe>();
                    ProductionRecipe oProductionRecipe = new ProductionRecipe();
                    oProductionRecipe.ErrorMessage = "Unit conversion required for [" +oItem.ProductCode+"]"+ oItem.ProductName + "!";
                    oTempProductionRecipes.Add(oProductionRecipe);
                    return oTempProductionRecipes;
                }
                nPerHangerReqQty = (nPerHangerReqQty * nConvertedValue);
                oItem.RequiredQty = nPerHangerReqQty * nFGQty;
            }
            else
            {
                oItem.RequiredQty = oItem.QtyInPercent * nFGQty;
            }
        }
        return oTempProductionRecipes;
    }

     public List<ProductionRecipe> CalculateRequiredQtyForPoly(List<ProductionRecipe> oTempProductionRecipes, Recipe oRecipe)
     {

         double nTotalWeight = Convert.ToDouble(oRecipe.Note.Split('~')[0]);
         double nFGQty = Convert.ToDouble(oRecipe.Note.Split('~')[1]);
         int nFGMUnitID = Convert.ToInt32(oRecipe.Note.Split('~')[2]);
         foreach (ProductionRecipe oItem in oTempProductionRecipes)
         {
             if (oItem.QtyTypeInt == 1)
             {
                 double nConvertedValue = 1;
                 double nPerPolyReqQty = (nTotalWeight * Convert.ToDouble(oItem.QtyInPercent / 100));
                 if (oItem.MUnitID != nFGMUnitID)
                 {
                     nConvertedValue = _oUnitConversions.Where(x => x.ProductID == oItem.ProductID && x.MeasurementUnitID == nFGMUnitID && x.ConvertedUnitID == oItem.MUnitID).Select(x => x.ConvertedUnitValue).FirstOrDefault();
                 }
                 if (nConvertedValue <= 0)
                 {
                     oTempProductionRecipes = new List<ProductionRecipe>();
                     ProductionRecipe oProductionRecipe = new ProductionRecipe();
                     oProductionRecipe.ErrorMessage = "Unit conversion required for [" + oItem.ProductCode + "]" + oItem.ProductName + "!";
                     oTempProductionRecipes.Add(oProductionRecipe);
                     return oTempProductionRecipes;
                 }
                 nPerPolyReqQty= (nPerPolyReqQty  * nConvertedValue);
                 oItem.RequiredQty = nPerPolyReqQty * nFGQty;
             }
             else
             {
                 oItem.RequiredQty = oItem.QtyInPercent * nFGQty;
             }
         }
         return oTempProductionRecipes;
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

        [HttpPost]
        public JsonResult SearchByProductBUModuleWise(Product oProduct)
        {
            _oProducts = new List<Product>();
            string sSQL = "";
            try
            {
                sSQL = "SELECT * ,ISNULL((SELECT NN.QtyType FROM RecipeDetail AS NN WHERE NN.RecipeDetailID = ISNULL((SELECT MAX(MM.RecipeDetailID) FROM RecipeDetail as MM WHERE ProductID = HH.ProductID),0)),0) As QtyType FROM View_Product AS HH ";
              if(oProduct.ProductName!=null && oProduct.ProductName!="")
              {  
                  sSQL += "WHERE HH.Activity = 1 AND HH.ProductName LIKE '%" + oProduct.ProductName + "%' AND  HH.ProductCategoryID IN (SELECT PC.ProductCategoryID FROM [dbo].[FN_GetProductCategoryByBUModuleAndUser](" + (int)((User)Session[SessionInfo.CurrentUser]).UserID + "," + (int)EnumModuleName.Recipe + "," + (int)EnumProductUsages.Regular + "," + oProduct.BUID + ") AS PC) ORDER BY HH.ProductName";
                  //_oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.Recipe, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
              }
              else
              {
                  sSQL += "WHERE HH.Activity = 1 AND  HH.ProductCategoryID IN (SELECT PC.ProductCategoryID FROM [dbo].[FN_GetProductCategoryByBUModuleAndUser](" + (int)((User)Session[SessionInfo.CurrentUser]).UserID + "," + (int)EnumModuleName.Recipe + "," + (int)EnumProductUsages.Regular + "," + oProduct.BUID + ") AS PC) ORDER BY HH.ProductName";
                  //_oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.Recipe, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
              }
              _oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
              if (_oProducts.Count() <= 0) throw new Exception("No product found.");
            }
            catch (Exception ex)
            {
                _oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
