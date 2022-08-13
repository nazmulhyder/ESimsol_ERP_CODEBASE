using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class ProductCategoryController : Controller
    {
        #region Declaration
        ProductCategory _oProductCategory = new ProductCategory();
        TProductCategory _oTProductCategory = new TProductCategory();
        
        List<ProductCategory> _oProductCategorys = new List<ProductCategory>();
        List<TProductCategory> _oTProductCategorys = new List<TProductCategory>();

        ProductCategoryProperty _oPCPI = new ProductCategoryProperty();
        List<ProductCategoryProperty> _oPCPIs = new List<ProductCategoryProperty>();
        string _sErrorMessage = "";
        #endregion



        #region Function
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
            return oTProductCategorys;
        }
        #endregion

        public ActionResult ViewProductCategorys(int BUID, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
           // this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'ProductCategory','Product'", (int)Session[SessionInfo.CurrentUser], ((User)(Session[SessionInfo.CurrentUser])).UserID));

            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();            
            try
            {
                if (BUID > 0)
                {
                    _oProductCategorys = ProductCategory.BUWiseGets(BUID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oProductCategorys = ProductCategory.Gets( ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                foreach (ProductCategory oItem in _oProductCategorys)
                {

                    _oTProductCategory = new TProductCategory();
                    _oTProductCategory.id = oItem.ProductCategoryID;
                    _oTProductCategory.text = oItem.ProductCategoryName;
                    _oTProductCategory.state = "";
                    _oTProductCategory.attributes = "";
                    _oTProductCategory.parentid = oItem.ParentCategoryID;
                    _oTProductCategory.Description =oItem.Note;
                    _oTProductCategory.IsLastLayer = oItem.IsLastLayer;
                    _oTProductCategory.DrAccountHeadID = oItem.DrAccountHeadID;
                    _oTProductCategory.DrAccountHeadName = oItem.DrAccountHeadName;
                    _oTProductCategory.CrAccountHeadID = oItem.CrAccountHeadID;
                    _oTProductCategory.CrAccountHeadName = oItem.CrAccountHeadName;
                   //_oTProductCategory.AssetTypeInString = oItem.AssetTypeInString;
                    _oTProductCategorys.Add(_oTProductCategory);
                }
                _oTProductCategory = new TProductCategory();
                _oTProductCategory = GetRoot(0);
                this.AddTreeNodes(ref _oTProductCategory);
                return View(_oTProductCategory);
            }

            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTProductCategory);
            }
        }
        
        public ActionResult ViewProductCategory(int id, int pid, double ts)
        {
            ProductCategory oProductCategory = new ProductCategory();
            ProductSetup oProductSetup = new ProductSetup();
            if (id > 0)
            {
                oProductCategory = oProductCategory.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_ProductCategory WHERE ParentCategoryID = " + id;
                oProductCategory.ProductCategorys = ProductCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oProductSetup = oProductSetup.GetByCategory(id, (int)Session[SessionInfo.currentUserID]);
                oProductCategory.BUWiseProductCategories = BUWiseProductCategory.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oProductCategory.ParentBUWiseProductCategories = BUWiseProductCategory.Gets(oProductCategory.ParentCategoryID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oProductCategory.ParentCategoryID = pid;
                oProductSetup = oProductSetup.GetByCategory(pid, (int)Session[SessionInfo.currentUserID]);
                oProductCategory.BUWiseProductCategories = new List<BUWiseProductCategory>();
                oProductCategory.ParentBUWiseProductCategories = BUWiseProductCategory.Gets(pid, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.ProductSetup = oProductSetup;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);    
            return View(oProductCategory);
        }
        public ActionResult ViewProductCategoryLastLayer(int id, int pid, double ts)
        {
            ProductCategory oProductCategory = new ProductCategory();
            if (id > 0)
            {
                oProductCategory = oProductCategory.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSQL = "SELECT * FROM View_ProductCategory WHERE ParentCategoryID = " + id;
                oProductCategory.ProductCategorys = ProductCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oProductCategory.IsLastLayer = true;
                oProductCategory.ParentCategoryID = pid;
            }
            ProductSetup oProductSetup = new ProductSetup();
            oProductSetup = oProductSetup.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductSetup = oProductSetup;
            return View(oProductCategory);
        }
        
        [HttpPost]
        public JsonResult Save(ProductCategory oProductCategory)
        {
            _oProductCategory = new ProductCategory();
            try
            {
                _oProductCategory = oProductCategory;
                if (_oProductCategory.ProductCategoryID <= 0)
                {
                    _oProductCategory = _oProductCategory.IUD(_oProductCategory.ProductCategoryID, (int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProductCategory = _oProductCategory.IUD(_oProductCategory.ProductCategoryID, (int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oProductCategory = new ProductCategory();
                _oProductCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAlcategory(ProductCategory oProductCategory)
        {
            _oProductCategorys = new List<ProductCategory>();                        
            try
            {
                _oProductCategorys = ProductCategory.Gets((int)Session[SessionInfo.currentUserID]);
                foreach (ProductCategory oItem in _oProductCategorys)
                {

                    _oTProductCategory = new TProductCategory();
                    _oTProductCategory.id = oItem.ProductCategoryID;
                    _oTProductCategory.text = oItem.ProductCategoryName;
                    _oTProductCategory.state = "";
                    _oTProductCategory.attributes = "";
                    _oTProductCategory.parentid = oItem.ParentCategoryID;
                    _oTProductCategory.Description = oItem.Note;
                    _oTProductCategory.IsLastLayer = oItem.IsLastLayer;                    
                    _oTProductCategorys.Add(_oTProductCategory);
                }
                _oTProductCategory = new TProductCategory();
                _oTProductCategory = GetRoot(0);
                this.AddTreeNodes(ref _oTProductCategory);                
            }
            catch (Exception ex)
            {
                _oTProductCategory = new TProductCategory();
                _oTProductCategory.text = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTProductCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAccountEffect(ProductCategory oProductCategory)
        {
            _oProductCategory = new ProductCategory();
            try
            {
                _oProductCategory = oProductCategory.Update_AccountHead((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductCategory = new ProductCategory();
                _oProductCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetByName(ProductCategory oProductCategory)
        {
            _oProductCategorys = new List<ProductCategory>();
            string sSQL = "SELECT * FROM VIEW_ProductCategory";
            string sReturn = "";
            try
            {
                #region Category Name
                if (!String.IsNullOrEmpty(oProductCategory.ProductCategoryName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductCategoryName like '%" + oProductCategory.ProductCategoryName + "%'";
                }
                #endregion

                #region Last Layer
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "IsLastLayer=1";
                #endregion

                #region BusinessUnit
                if(oProductCategory.BusinessUnitID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductCategoryID IN  (SELECT HH.ProductCategoryID FROM  BUWiseProductCategory AS HH WHERE HH.BUID = " + oProductCategory.BusinessUnitID.ToString() + ")";
                }
                #endregion

                sSQL = sSQL + sReturn;
                _oProductCategorys = ProductCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductCategorys = new List<ProductCategory>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getchildren(int parentid)
        {
            _oProductCategorys = new List<ProductCategory>();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();
            try
            {                
                string sSQL = "SELECT * FROM VIEW_ProductCategory WHERE ParentCategoryID=" + parentid.ToString();
                _oProductCategorys = ProductCategory.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oProductCategory = new ProductCategory();
                _oProductCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sfeedbackmessage = "";
            _oProductCategory = new ProductCategory();
            ProductCategory oProductCategory = new ProductCategory();
            try
            {
                _oProductCategory = _oProductCategory.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oProductCategory = _oProductCategory.IUD(_oProductCategory.ProductCategoryID, (int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
                if (oProductCategory.ErrorMessage == "")
                {
                    sfeedbackmessage = "Data Delete Successfully";
                }
                else
                {
                    sfeedbackmessage = oProductCategory.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                sfeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditProductCategory(int id)
        {
            _oProductCategory = new ProductCategory();
            _oProductCategory = _oProductCategory.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
          //  _oProductCategory.AssetTypeInInt = (int)_oProductCategory.AssetType;
            return PartialView(_oProductCategory);
        }

        #region ProductCategory Property
        public ActionResult SetProperty(int id) //ProductCAtegoryID
        {
            _oPCPI = new ProductCategoryProperty();
            ProductCategory oPC = new ProductCategory();
            oPC = oPC.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oPCPI.ProductCategoryID = id;
            _oPCPI.ProductCategoryName = oPC.ProductCategoryName;
            //Load Product Property  in Combo
            List<Property> oPropertys = new List<Property>();
            Property oProperty = new Property();
            oProperty.PropertyName = "--- Select Property Name--";
            oPropertys.Add(oProperty);
            string sSQL1 = "SELECT * FROM View_Property";
            oPropertys.AddRange(Property.Gets(sSQL1, ((User)(Session[SessionInfo.CurrentUser])).UserID));
            _oPCPI.Properties = oPropertys;
            _oPCPI.PCPIs = ProductCategoryProperty.Gets(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sSQL = "SELECT * FROM View_PropertyValue";
            _oPCPI.PropertyValueList = PropertyValue.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oPCPI);
        }

        [HttpPost]
        public JsonResult PCPIInsert(ProductCategoryProperty oPCPI)
        {
            _oPCPI = new ProductCategoryProperty();
            try
            {
                _oPCPI = oPCPI;
                _oPCPI = _oPCPI.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oPCPI = new ProductCategoryProperty();
                _oPCPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPCPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PCPIDelete(ProductCategoryProperty oPCPI)//Invoice Product
        {
            _oPCPI = new ProductCategoryProperty();
            try
            {
                _oPCPI = oPCPI;
                _oPCPI = _oPCPI.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oPCPI = new ProductCategoryProperty();
                _oPCPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPCPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPCPV(int nid) //PCPID
        {
            List<ProductCategoryPropertyValue> oPCPVs = new List<ProductCategoryPropertyValue>();
            try
            {
                oPCPVs = ProductCategoryPropertyValue.Gets(nid, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                ProductCategoryPropertyValue oPCPV = new ProductCategoryPropertyValue();
                oPCPV.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((List<ProductCategoryPropertyValue>)oPCPVs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPCPV(ProductCategoryProperty oPCP) //
        {
            string Message = "";
            ProductCategoryPropertyValue oPCPV = new ProductCategoryPropertyValue();
            try
            {
                Message = oPCPV.Insert(oPCP, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oPCPI = new ProductCategoryProperty();
                _oPCPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Message);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Old Task

        #region Functions


        private bool ValidateInput(ProductCategory oProductCategory)
        {
            if (oProductCategory.ProductCategoryName == null || oProductCategory.ProductCategoryName == "")
            {
                _sErrorMessage = "Please enter Product Category Name";
                return false;
            }
            if (oProductCategory.ParentCategoryID <= 0)
            {
                _sErrorMessage = "Invalid Parent Product Category try again";
                return false;
            }
            if (oProductCategory.IsLastLayer)
            {
                //if (oProductCategory.PTM.LotTableName == null)
                //{
                //    _sErrorMessage = "Please enter Lot Table Name";
                //    return false;
                //}
                //if (oProductCategory.PTM.ITTableName == null)
                //{
                //    _sErrorMessage = "Please enter ITransaction Table Name";
                //    return false;
                //}

                //if (oProductCategory.PTM.LotTableName == "")
                //{
                //    _sErrorMessage = "Please enter Lot Table Name";
                //    return false;
                //}
                //if (oProductCategory.PTM.ITTableName == "")
                //{
                //    _sErrorMessage = "Please enter ITransaction Table Name";
                //    return false;
                //}

            }
            return true;
        }

     
        #endregion
        
        public ActionResult Add(int ParentID)
        {
            if (ParentID <= 0)
            {
                return RedirectToAction("RefreshList");
            }
            ProductCategory oTempProductCategory = new ProductCategory();
            oTempProductCategory = oTempProductCategory.Get(ParentID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ProductCategory oProductCategory = new ProductCategory();
            oProductCategory.ParentCategoryID = ParentID;
          //  oProductCategory.SelectedParentCategory = "Selected Parent Category : " + oTempProductCategory.ProductCategoryName;
            return View(oProductCategory);
        }

        public ActionResult RefreshList()
        {
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            try
            {
                _oProductCategorys = ProductCategory.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oProductCategory = GetRoot1();
                this.AddTreeNodes1(ref _oProductCategory);
                return View(_oProductCategory);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oProductCategory);
            }
        }

        private void AddTreeNodes1(ref ProductCategory oProductCategory)
        {
            IEnumerable<ProductCategory> oChildCategorys;
            oChildCategorys = GetChild1(oProductCategory.ProductCategoryID);
            oProductCategory.ChildCategorys = oChildCategorys;

            foreach (ProductCategory oItem in oChildCategorys)
            {
                ProductCategory oTemp = oItem;
                AddTreeNodes1(ref oTemp);
            }
        }

        private IEnumerable<ProductCategory> GetChild1(int nProductCategoryID)
        {
            List<ProductCategory> oProductCategorys = new List<ProductCategory>();
            foreach (ProductCategory oItem in _oProductCategorys)
            {
                if (oItem.ParentCategoryID == nProductCategoryID)
                {
                    oProductCategorys.Add(oItem);
                }
            }
            return oProductCategorys;
        }

        private ProductCategory GetRoot1()
        {
            ProductCategory oProductCategory = new ProductCategory();
            foreach (ProductCategory oItem in _oProductCategorys)
            {
                if (oItem.ParentCategoryID == 0)
                {
                    return oItem;
                }
            }
            return oProductCategory;
        }
        public ActionResult RefreshListForMenu()
        {
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            try
            {
                _oProductCategorys = ProductCategory.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oProductCategory = GetRoot1();
                this.AddTreeNodes1(ref _oProductCategory);
                return PartialView(_oProductCategory);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return PartialView(_oProductCategory);
            }
        }

        [HttpPost]
        public ActionResult Add(ProductCategory oProductCategory)
        {
            try
            {
                if (this.ValidateInput(oProductCategory))
                {
                    _oProductCategory = _oProductCategory.IUD(_oProductCategory.ProductCategoryID, (int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                    _sErrorMessage = oProductCategory.ErrorMessage;
                    if (_sErrorMessage != "")
                    { 
                        TempData["message"] = _sErrorMessage.Split('!')[0];
                        return View(oProductCategory);
                    }
                    return RedirectToAction("RefreshList");
                }
                TempData["message"] = _sErrorMessage;
                ProductCategory oParent = new ProductCategory();
                oParent = oParent.Get(oProductCategory.ParentCategoryID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
               // oProductCategory.SelectedParentCategory = "Selected parent Product Category : " + oParent.ProductCategoryName;
                return View(oProductCategory);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(oProductCategory);
            }
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("RefreshList");
            }
            ProductCategory oProductCategory = new ProductCategory();
            oProductCategory = oProductCategory.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oProductCategory);
        }

     //   [HttpPost]
        //public ActionResult Edit(ProductCategory oProductCategory)
        //{
        //    try
        //    {
        //        if (this.ValidateInput(oProductCategory))
        //        {
        //            oProductCategory = oProductCategory.IUD(EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //            _sErrorMessage = oProductCategory.ErrorMessage;
        //            if (_sErrorMessage != "")
        //            {
        //                TempData["message"] = _sErrorMessage.Split('!')[0];
        //                return View(oProductCategory);
        //            }
        //            return RedirectToAction("RefreshList");
        //        }
        //        TempData["message"] = _sErrorMessage;
        //        return View(oProductCategory);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["message"] = ex.Message;
        //        return View(oProductCategory);
        //    }
        //}

     

     

        // Modified On 3rd February By Fauzul To Load Product Category on Combo to Product Packing List
        //[HttpGet]
        //public JsonResult LoadComboProductPackingList()
        //{
        //    List<ProductCategory> oProductCategorys = new List<ProductCategory>();
        //    ProductCategory oProductCategory = new ProductCategory();
        //    oProductCategory.ProductCategoryName = "-- Select Product --";
        //    oProductCategorys.Add(oProductCategory);
        //    oProductCategorys.AddRange(ProductCategory.GetsforPPL(((User)(Session[SessionInfo.CurrentUser])).UserID));

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize((object)oProductCategorys);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        
        //public ActionResult ProductCategoryPicker(int nProductCategoryID)
        //{
        //    ProductCategory oProductCategory = new ProductCategory();
        //    string sSQL = "SELECT * FROM View_ProductCategoryGroupLookup WHERE PCGID IN (SELECT PCGID FROM View_ProductCategoryGroupLookup WHERE ProductCategoryID = " + nProductCategoryID + ") AND ProductCategoryID <> " + nProductCategoryID + "";
        //    oProductCategory.ProductCategoryGroupLookups = ProductCategoryGroupLookup.Gets(sSQL,((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    return PartialView(oProductCategory);
        //}
        
        public ActionResult ProductCategoryTree()
        {
            ProductCategory oProductCategory = new ProductCategory();
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetsProductCategoryTreeByParent(ProductCategory oProductCategory)
        {
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();

            _oProductCategorys = ProductCategory.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            foreach (ProductCategory oItem in _oProductCategorys)
            {

                _oTProductCategory = new TProductCategory();
                _oTProductCategory.id = oItem.ProductCategoryID;
                _oTProductCategory.parentid = oItem.ParentCategoryID;
                _oTProductCategory.text = oItem.ProductCategoryName;
                _oTProductCategory.attributes = oItem.IsLastLayer.ToString();// +'~' + oItem.AssetType + '~' + oItem.AssetTypeInString.ToString();
                _oTProductCategory.Description = oItem.Note;
                _oTProductCategory.IsLastLayer = oItem.IsLastLayer;

                _oTProductCategorys.Add(_oTProductCategory);
            }
            _oTProductCategory = new TProductCategory();
            _oTProductCategory = GetParentByID(oProductCategory.ParentCategoryID);
            this.AddTreeNodes(ref _oTProductCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oTProductCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



   
        [HttpPost]
        public JsonResult GetsProductCategoryTree()
        {
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();

            _oProductCategorys = ProductCategory.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            foreach (ProductCategory oItem in _oProductCategorys)
            {

                _oTProductCategory = new TProductCategory();
                _oTProductCategory.id = oItem.ProductCategoryID;
                _oTProductCategory.parentid = oItem.ParentCategoryID;
                _oTProductCategory.text = oItem.ProductCategoryName;
                _oTProductCategory.attributes = oItem.IsLastLayer.ToString();// +'~' + oItem.AssetType + '~' + oItem.AssetTypeInString.ToString();
                _oTProductCategory.Description = oItem.Note;
                _oTProductCategory.IsLastLayer = oItem.IsLastLayer;
                
                _oTProductCategorys.Add(_oTProductCategory);
            }
            _oTProductCategory = new TProductCategory();
            _oTProductCategory = GetRoot(0);
            this.AddTreeNodes(ref _oTProductCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oTProductCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsCategoryByParent(ProductCategory oProductCategory)
        {
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();

            _oProductCategorys = ProductCategory.GetsByParentID(oProductCategory.ParentCategoryID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            foreach (ProductCategory oItem in _oProductCategorys)
            {

                _oTProductCategory = new TProductCategory();
                _oTProductCategory.id = oItem.ProductCategoryID;
                _oTProductCategory.parentid = oItem.ParentCategoryID;
                _oTProductCategory.text = oItem.ProductCategoryName;
                _oTProductCategory.attributes = oItem.IsLastLayer.ToString();// +'~' + oItem.AssetType + '~' + oItem.AssetTypeInString.ToString();
                _oTProductCategory.Description = oItem.Note;
                _oTProductCategory.IsLastLayer = oItem.IsLastLayer;

                _oTProductCategorys.Add(_oTProductCategory);
            }
            _oTProductCategory = new TProductCategory();
            _oTProductCategory = GetRoot(0);
            this.AddTreeNodes(ref _oTProductCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oTProductCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        
      
        #endregion

        #region Gets for ComboTree
        [HttpPost]
        public JsonResult GetsProductCategoryForCombo(ProductCategory oProductCategory)
        {
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategory = new ProductCategory();
            _oTProductCategory = new TProductCategory();
            _oTProductCategorys = new List<TProductCategory>();
            List<TProductCategory> oTProductCategorys = new List<TProductCategory>();
            if (oProductCategory.BusinessUnitID == null) { oProductCategory.BusinessUnitID = 0; }
            if (oProductCategory.BusinessUnitID > 0)
            {
                _oProductCategorys = ProductCategory.BUWiseGets(oProductCategory.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oProductCategorys = ProductCategory.Gets((int)Session[SessionInfo.currentUserID]);
            }
            foreach (ProductCategory oItem in _oProductCategorys)
            {
                _oTProductCategory = new TProductCategory();
                _oTProductCategory.id = oItem.ProductCategoryID;
                _oTProductCategory.text = oItem.ProductCategoryName;
                if (!oItem.IsLastLayer)
                {
                    _oTProductCategory.state = "closed";
                }
                else
                {
                    _oTProductCategory.state = "";
                }
                _oTProductCategory.attributes = "";
                _oTProductCategory.parentid = oItem.ParentCategoryID;
                _oTProductCategory.Description = oItem.Note;
                _oTProductCategory.IsLastLayer = oItem.IsLastLayer;
                _oTProductCategory.IsApplyGroup = oItem.IsApplyGroup;
                _oTProductCategorys.Add(_oTProductCategory);
            }
            oTProductCategorys = new List<TProductCategory>();
            oTProductCategorys = GetRoots(1);
            foreach (TProductCategory oItem in oTProductCategorys)
            {
                TProductCategory oTProductCategory = oItem;
                this.AddTreeNodes(ref oTProductCategory);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTProductCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<TProductCategory> GetRoots(int nParentID)
        {
            TProductCategory oTProductCategory = new TProductCategory();
            List<TProductCategory> oTProductCategorys = new List<TProductCategory>();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.parentid == nParentID)
                {
                    oTProductCategorys.Add(oItem);
                }
            }
            return oTProductCategorys;
        }
        #endregion

    }
}
