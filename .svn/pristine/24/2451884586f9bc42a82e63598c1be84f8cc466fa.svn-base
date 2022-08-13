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
using System.Data;
using System.Data.OleDb;

namespace ESimSolFinancial.Controllers
{
    public class KnitDyeingRecipeController : Controller
    {
        public ActionResult ViewKnitDyeingRecipes(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KnitDyeingRecipe).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            List<KnitDyeingRecipe> _oKnitDyeingRecipe = new List<KnitDyeingRecipe>();
            string sSQL = "SELECT * FROM View_KnitDyeingRecipe ORDER BY KnitDyeingRecipeID ASC";
            _oKnitDyeingRecipe = KnitDyeingRecipe.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oKnitDyeingRecipe);

        }
        public ActionResult ViewKnitDyeingRecipe(int id)
        {

            KnitDyeingRecipe _oKnitDyeingRecipe = new KnitDyeingRecipe();
            if (id > 0)
            {
                _oKnitDyeingRecipe = _oKnitDyeingRecipe.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<KnitDyeingRecipeDetail> oKnitDyeingRecipeDetails = new List<KnitDyeingRecipeDetail>();
                oKnitDyeingRecipeDetails = KnitDyeingRecipeDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                _oKnitDyeingRecipe.KnitDyeingRecipeDetails = oKnitDyeingRecipeDetails;
            }

            return View(_oKnitDyeingRecipe);

        }

        [HttpPost]
        public JsonResult Save(KnitDyeingRecipe oKnitDyeingRecipe)
        {
            KnitDyeingRecipe _oKnitDyeingRecipe = new KnitDyeingRecipe();
            try
            {

                _oKnitDyeingRecipe = oKnitDyeingRecipe.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingRecipe = new KnitDyeingRecipe();
                _oKnitDyeingRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(KnitDyeingRecipe oKnitDyeingRecipe)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oKnitDyeingRecipe.Delete(oKnitDyeingRecipe.KnitDyeingRecipeID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Activity(KnitDyeingRecipe oKnitDyeingRecipe)
        {
            KnitDyeingRecipe _oKnitDyeingRecipe = new KnitDyeingRecipe();
            try
            {
                _oKnitDyeingRecipe = oKnitDyeingRecipe.Activity((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oKnitDyeingRecipe = new KnitDyeingRecipe();
                _oKnitDyeingRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnitDyeingRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnitDyeingRecipe, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnitDyeingRecipe, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                Product _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public JsonResult GetMUnit(Product oProduct)
        {
            List<Product> _oProducts = new List<Product>();
            List<MeasurementUnit> _oMeasurementUnits = new List<MeasurementUnit>();
            try
            {
                _oMeasurementUnits = MeasurementUnit.Gets(oProduct.UnitType, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                MeasurementUnit _oMeasurementUnit = new MeasurementUnit();
                _oMeasurementUnit.ErrorMessage = ex.Message;
                _oMeasurementUnits.Add(_oMeasurementUnit);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeasurementUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

    }
}