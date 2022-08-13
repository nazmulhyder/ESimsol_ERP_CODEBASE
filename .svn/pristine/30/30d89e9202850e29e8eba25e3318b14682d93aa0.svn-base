using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Linq;

namespace ESimSolFinancial.Controllers
{
    public class ProductBaseController : Controller
    {
        #region Declaration
        ProductBase _oProductBase = new ProductBase();
        List<ProductBase> _oProductBases = new List<ProductBase>();
        List<Property> _oPropertys = new List<Property>();
        List<PropertyValue> _oPropertyValues = new List<PropertyValue>();
        List<Product> _oProducts = new List<Product>();
        Product _oProduct = new Product();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        ProductCategory _oProductCategory = new ProductCategory();
        TProductCategory _oTProductCategory = new TProductCategory();

        List<ProductCategory> _oProductCategorys = new List<ProductCategory>();
        List<TProductCategory> _oTProductCategorys = new List<TProductCategory>();
        string _sErrorMessage = "";
        #endregion


        #region Tree Function
        private TProductCategory GetRoot(int nParentCategoryID)
        {
            TProductCategory oTProductCategory = new TProductCategory();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.parentid == nParentCategoryID)
                {
                    return oItem;
                }
            }
            return _oTProductCategory;
        }


        private TProductCategory GetParentByID(int nParentCategoryID)
        {
            TProductCategory oTProductCategory = new TProductCategory();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.id == nParentCategoryID)
                {
                    return oItem;
                }
            }
            return _oTProductCategory;
        }

        private void AddTreeNodes(ref TProductCategory oTProductCategory)
        {
            IEnumerable<TProductCategory> oChildNodes;
            oChildNodes = GetChild(oTProductCategory.id);
            oTProductCategory.children = oChildNodes;
            if(oChildNodes.Count()>0)
            {
                oTProductCategory.state = "closed";
            }
            else
            {
                oTProductCategory.state = "";
            }

            foreach (TProductCategory oItem in oChildNodes)
            {
                TProductCategory oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private IEnumerable<TProductCategory> GetChild(int nParentCategoryID)
        {
            List<TProductCategory> oTProductCategorys = new List<TProductCategory>();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.parentid == nParentCategoryID)
                {
                    oTProductCategorys.Add(oItem);
                }
            }
            return oTProductCategorys.OrderBy(x=>x.text).ToList();
        }
        #endregion

        #region Functions
        private bool ValidateInput(ProductBase oProductBase)
        {
            if (oProductBase.ProductCategoryID <= 0)
            {
                _sErrorMessage = "Please select product category";
                return false;
            }
            if (oProductBase.ProductName == null || oProductBase.ProductName =="")
            {
                _sErrorMessage = "Please enter product Name";
                return false;
            }
            if (oProductBase.ShortName == null || oProductBase.ShortName == "")
            {
                _sErrorMessage = "Please enter Invoice Product Name";
                return false;
            }
          
            return true;
        }
        [HttpPost]
        public JsonResult Save(ProductBase oProductBase)
        {
            _oProductBase = new ProductBase();
            try
            {
                _oProductBase = oProductBase;
                _oProductBase = _oProductBase.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductBase = new ProductBase();
                _oProductBase.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductBase);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(ProductBase oProductBase)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oProductBase.Delete(oProductBase.ProductBaseID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Action
        public ActionResult ViewProductBases(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            _oClientOperationSetting = new ClientOperationSetting();

            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();            
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'ProductBase', 'Property'", (int)Session[SessionInfo.currentUserID], (Guid)Session[SessionInfo.wcfSessionID]));
            
            _oProductBases = new List<ProductBase>();
            if (buid > 0)
            {
                _oProductCategorys = ProductCategory.BUWiseGets(buid, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                _oProductCategorys = ProductCategory.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            foreach (ProductCategory oItem in _oProductCategorys)
            {

                _oTProductCategory = new TProductCategory();
                _oTProductCategory.id = oItem.ProductCategoryID;
                _oTProductCategory.text = oItem.ProductCategoryName;
                _oTProductCategory.attributes = "";
                _oTProductCategory.parentid = oItem.ParentCategoryID;

                _oTProductCategory.Description = oItem.Note;
                _oTProductCategory.IsLastLayer = oItem.IsLastLayer;
                _oTProductCategory.IsApplyGroup = oItem.IsApplyGroup;
                _oTProductCategory.ApplyProductType_IsShow = oItem.ApplyProductType_IsShow;
                _oTProductCategory.ApplyProperty_IsShow = oItem.ApplyProductType_IsShow;
                _oTProductCategory.ApplyPlantNo_IsShow = oItem.ApplyProductType_IsShow;
                _oTProductCategory.IsApplyCategory = oItem.ApplyProductType_IsShow;
                _oTProductCategory.ApplyGroup_IsShow = oItem.ApplyProductType_IsShow;
                _oTProductCategory.DrAccountHeadID = oItem.DrAccountHeadID;
                _oTProductCategory.DrAccountHeadName = oItem.DrAccountHeadName;
                _oTProductCategory.CrAccountHeadID = oItem.CrAccountHeadID;
                _oTProductCategory.CrAccountHeadName = oItem.CrAccountHeadName;
                _oTProductCategorys.Add(_oTProductCategory);
            }
            _oTProductCategory = new TProductCategory();
            _oTProductCategory = GetRoot(0);
            this.AddTreeNodes(ref _oTProductCategory);
             _oTProductCategory.state = "";

            ViewBag.TProductCategory = _oTProductCategory;

            ViewBag.UniteTypeObj = EnumObject.jGets(typeof(EnumUniteType));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductTypeObjs = EnumObject.jGets(typeof(EnumProductType));
            ViewBag.BUID = buid;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProductCodeManual, (int)Session[SessionInfo.currentUserID]);
            return View(_oProductBases);
        }
        public ActionResult ViewProductBase(int pcid, int id, double ts)
        {
            _oProductBase = new ProductBase();
            ProductCategory oProductCategory = new ProductCategory();
            if (id > 0)
            {
                _oProductBase = _oProductBase.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oProductCategory = oProductCategory.Get(pcid, (int)Session[SessionInfo.currentUserID]);
                _oProductBase.ProductCategoryID = pcid;
                _oProductBase.ProductCategoryName = oProductCategory.ProductCategoryName;
            }
            return View(_oProductBase);
        }
        [HttpPost]
        public JsonResult GetByCategory(ProductBase oProductBase)
        {
            _oProductBases = new List<ProductBase>();
            try
            {
                _oProductBases = ProductBase.GetsByCategory(oProductBase.ProductCategoryID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductBases = new List<ProductBase>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductBases);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByCategoryOrName(ProductBase oProductBase)
        {
            _oProductBases = new List<ProductBase>();
            string sSQL = "SELECT * FROM View_ProductBase";
            string sReturn = "";
            try
            {
                if (oProductBase.ProductCategoryID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductCategoryID="+ oProductBase.ProductCategoryID+"";
                }
                if (!String.IsNullOrEmpty(oProductBase.ProductName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductName like '%" + oProductBase.ProductName + "%'";
                }

                sSQL = sSQL + sReturn;

                _oProductBases = ProductBase.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductBases = new List<ProductBase>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductBases);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
      
       

        public ActionResult ProductPiker()
        {
            _oProductBases = new List<ProductBase>();
            _oProductBases = ProductBase.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oProductBases);
        }
        // To show Measurement Unit in a ComboBox in Add view by Fauzul on April 3, 2013
        [HttpGet]
        public JsonResult GetByMeasurementID(int id)
        {
            MeasurementUnit oMeasurementUnit = new MeasurementUnit();
            oMeasurementUnit = oMeasurementUnit.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMeasurementUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        // Modified On 3rd Arpril 2013 By Fauzul To Load Product Category on Combo
        [HttpGet]
        public JsonResult LoadComboProductCategory()
        {
            List<ProductCategory> oProductCategorys = new List<ProductCategory>();
            ProductCategory oProductCategory = new ProductCategory();
            oProductCategory.ProductCategoryName = "-- Select ProductCategory --";
            oProductCategorys.Add(oProductCategory);
           // oProductCategorys.AddRange(ProductCategory.GetsforPPL(((User)(Session[SessionInfo.CurrentUser])).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadComboProductName() 
        {
            List<ProductBase> oProductBases = new List<ProductBase>();
            ProductBase oProductBase = new ProductBase();
            oProductBase.ProductName  = "-- Select Product Name --";
            oProductBases.Add(oProductBase);
            oProductBases.AddRange(ProductBase.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductBases);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //The following action will be needed for bulk item entry for single category.
      
        #endregion

        #region Searching
        //public ActionResult Searching()
        //{
        //    ProductBase oProductBase = new ProductBase();
        //    _oPropertys = new List<Property>();
        //    _oPropertyValues = new List<PropertyValue>();

        //    _oPropertys = Property.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    _oPropertyValues = PropertyValue.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    for (int i = 0; i < _oPropertys.Count; i++)
        //    {
        //        _oPropertys[i].PropertyValues = GetsPropertyValue(_oPropertys[i].PropertyID);
        //    }
        //    //oProductBase.Propertys = _oPropertys;
        //    return PartialView(oProductBase);
        //}

        //private List<PropertyValue> GetsPropertyValue(int nPropertyID)
        //{
        //    List<PropertyValue> oPropertyValues = new List<PropertyValue>();
        //    foreach (PropertyValue oItem in _oPropertyValues)
        //    {
        //        if (oItem.PropertyID == nPropertyID)
        //        {
        //            oPropertyValues.Add(oItem);
        //        }
        //    }
        //    return oPropertyValues;
        //}

        [HttpPost]
        public JsonResult Gets(ProductBase oProductBase)
        {
            List<ProductBase> oProductBases = new List<ProductBase>();
            try
            {
                string sSQL = GetSQL(oProductBase);
                oProductBases = ProductBase.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oProductBases = new List<ProductBase>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductBases);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(ProductBase oProductBase)
        {
            //if (oProductBase.ProductCategoryIDs == null)
            //{
            //    oProductBase.ProductCategoryIDs = "";
            //}
            if (oProductBase.ProductName == null)
            {
                oProductBase.ProductName = "";
            }
            if (oProductBase.ProductCode == null)
            {
                oProductBase.ProductCode = "";
            }
            if (oProductBase.PropertyIDs == null)
            {
                oProductBase.PropertyIDs = "";
            }
            if (oProductBase.PropertyValueIDs == null)
            {
                oProductBase.PropertyValueIDs = "";
            }

            string sProductCategoryIDs = "";

            //if (oProductBase.ProductCategoryIDs.Length > 0)
            //{
            //    sProductCategoryIDs = oProductBase.ProductCategoryIDs.Remove(oProductBase.ProductCategoryIDs.Length - 1, 1);
            //}
            if (oProductBase.PropertyIDs.Length > 0)
            {
                oProductBase.PropertyIDs = oProductBase.PropertyIDs.Remove(oProductBase.PropertyIDs.Length - 1, 1);
            }
            if (oProductBase.PropertyValueIDs.Length > 0)
            {
                oProductBase.PropertyValueIDs = oProductBase.PropertyValueIDs.Remove(oProductBase.PropertyValueIDs.Length - 1, 1);
            }

            string sReturn1 = "select * from View_ProductBase";
            string sReturn = "";


            if (oProductBase.ProductName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductName like '" + oProductBase.ProductName + '%' + "'";

            }

            if (oProductBase.ProductCode != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductCode = '" + oProductBase.ProductCode + "'";

            }

            if (sProductCategoryIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductCategoryID IN(" + sProductCategoryIDs + ')' + "";
            }

            if (oProductBase.PropertyValueIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductBaseID IN (SELECT DISTINCT ProductBaseID FROM ProductPropertyInformation WHERE PropertyValueID IN(" + oProductBase.PropertyValueIDs + "))";
            }

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        public ActionResult SimpleProductBaseSearching()
        {
            ProductBase oProductBase = new ProductBase();
            return PartialView(oProductBase);
        }
        #endregion
    }
}
