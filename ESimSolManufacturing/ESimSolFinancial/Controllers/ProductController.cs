using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using Newtonsoft.Json;


namespace ESimSolFinancial.Controllers
{
    public class ProductController : Controller
    {
        #region Declaration
        Property _oProperty = new Property();
        List<Property> _oPropertys = new List<Property>();
        PropertyValue _oPropertyValue = new PropertyValue();
        List<PropertyValue> _oPropertyValues = new List<PropertyValue>();
        ProductCategory _oProductCategory = new ProductCategory();
        List<ProductCategory> _oProductCategorys = new List<ProductCategory>();
        ProductBase _oProductBase = new ProductBase();
        List<ProductBase> _oProductBases = new List<ProductBase>();
        Product _oProduct = new Product();
        List<Product> _oProducts = new List<Product>();
        ProductTree _oProductTree = new ProductTree();
        List<ProductTree> _oProductTrees = new List<ProductTree>();
        ProductPropertyInformation _oProductPropertyInformation = new ProductPropertyInformation();
        List<ProductPropertyInformation> _oProductPropertyInformations = new List<ProductPropertyInformation>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        string _sErrorMessage = "";
        int i = 1;
        #endregion

        #region Functions

        public ActionResult ViewProducts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            
            _oProducts = new List<Product>();
            ProductCategory oProductCategory = new ProductCategory();
            ViewBag.UniteTypeObj = EnumObject.jGets(typeof(EnumUniteType));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductTypeObjs = EnumObject.jGets(typeof(EnumProductType));
            oProductCategory = oProductCategory.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductCategory = oProductCategory;
            ViewBag.BUID = buid;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProductCodeManual, (int)Session[SessionInfo.currentUserID]);
            return View(_oProducts);
        }

        [HttpPost]
        public JsonResult GetsInitializeData(BusinessUnit oBusinessUnit)
        {
            _oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM View_Product";
                if (oBusinessUnit.BusinessUnitID > 0)
                {
                    sSQL = sSQL + " WHERE ProductCategoryID IN (SELECT HH.ProductCategoryID FROM BUWiseProductCategory AS HH WHERE HH.BUID=" + oBusinessUnit.BusinessUnitID.ToString() + ") AND ISNULL(Activity,0) = 1 order by ProductCategoryID,ProductName";
                }
                _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProducts = new List<Product>();
            }
            var jsonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ViewProducts_Base(int nid, double ts)
        {
            ProductBase oProductBase = new ProductBase();
             ProductCategory oPC = new ProductCategory();
            oProductBase = oProductBase.Get(nid, (int)Session[SessionInfo.currentUserID]);
            _oProducts = new List<Product>();
            //_oProducts = Product.Getsby(nid, (int)Session[SessionInfo.currentUserID]);

            _oProducts = Product.Gets("SELECT top(1000)* FROM View_Product WHERE ProductBaseID="+nid, (int)Session[SessionInfo.currentUserID]);

            oPC = oPC.Get(oProductBase.ProductCategoryID, (int)Session[SessionInfo.currentUserID]);

            _oProduct.Products = _oProducts;
            _oProduct.ProductBaseID = nid;
            _oProduct.GroupName = oProductBase.ProductName;
            _oProduct.ProductCategoryName = " Group :" + oProductBase.ProductCategoryName + ", Category :" + oProductBase.ProductNameandCode;
            ViewBag.ProductCategory = oPC;
            ViewBag.UniteTypeObj = EnumObject.jGets(typeof(EnumUniteType));
            ViewBag.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FinishUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductTypeObjs = EnumObject.jGets(typeof(EnumProductType));
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProductCodeManual, (int)Session[SessionInfo.currentUserID]);
            return View(_oProduct);
        }
        public ActionResult ViewProduct_Base(int nbid, int id) //Product  ID
        {
            _oProduct = new Product();
            ProductBase oProductBase = new ProductBase();
            oProductBase = oProductBase.Get(nbid, (int)Session[SessionInfo.currentUserID]);

            if (id > 0)
            {
                _oProduct = _oProduct.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oProduct.ProductBaseID = nbid;
                _oProduct.GroupName = oProductBase.ProductName;
            }
            //_oProduct.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            //_oProduct.ProductTypeObjs = EnumObject.jGets(typeof(EnumProductType));

            ViewBag.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductTypeObjs = EnumObject.jGets(typeof(EnumProductType));
            return View(_oProduct);
        }
        public ActionResult ViewProduct_Category(int npcid, int id) //Product  ID
        {
            _oProduct = new Product();
            ProductCategory oProductCategory = new ProductCategory();
            oProductCategory = oProductCategory.Get(npcid, (int)Session[SessionInfo.currentUserID]);

            if (id > 0)
            {
                _oProduct = _oProduct.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oProduct.ProductCategoryID = npcid;
                _oProduct.ProductCategoryName = oProductCategory.ProductCategoryName;
            }
            _oProduct.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProduct);
        }
        [HttpPost]
        public JsonResult GetByCategory(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                _oProducts = Product.GetsByPCategory(oProduct.ProductCategoryID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProducts = new List<Product>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      

        [HttpPost]
        public JsonResult Save(Product oProduct)
        {
            _oProduct = new Product();
            try
            {
                oProduct.UnitType = (EnumUniteType)oProduct.UnitTypeInInt;
                oProduct.ProductType = (EnumProductType)oProduct.ProductTypeInInt;
                _oProduct = oProduct.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Product oProduct)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oProduct.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMultiple(Product oProduct)
        {
            string sErrorMease = "";
            List<Product> oTempProducts = new List<Product>();
            foreach (Product oItem in oProduct.Products)
            {
                sErrorMease = oItem.Delete((int)Session[SessionInfo.currentUserID]);
                if (sErrorMease != Global.DeleteMessage)
                {
                    oItem.ErrorMessage = sErrorMease;
                    oTempProducts.Add(oItem);
                }
               
            }
            var oProds = oProduct.Products.Where(x => !oTempProducts.Any(p => p.ProductID == x.ProductID)).ToList();
           
            if (oTempProducts.Any())
            {
                sErrorMease = "Partially Deleted";
              

            }
            else{
                 sErrorMease=Global.DeleteMessage;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(new { ErrorMease = sErrorMease, Products = oProds, NonDeleteProducts = oTempProducts });
            //PrintExcelForProduct(oTempProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
           
        }
        public void PrintExcelForProduct(FormCollection data)
        {
            string sProducts = data["Products"];
            List<Product> oProducts = JsonConvert.DeserializeObject<List<Product>>(sProducts);
            int nRowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Product List");
                sheet.Name = "Products";
                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 20; //Product Code
                sheet.Column(4).Width = 20; //Product Name
                sheet.Column(5).Width = 60; //ErrorMessage



                #region Report Header

                sheet.Cells[nRowIndex, 2, nRowIndex, 9].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Red);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Red);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Red);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Reasoning"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Red);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
                List<ContractorAddress> oContractorAddresss = new List<ContractorAddress>();

                #region Report Data
                int nCount = 0;
                foreach (Product oItem in oProducts)
                {
                    nCount++;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "@";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;

                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ErrorProduct.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        [HttpPost]
        public JsonResult GetsByCodeOrName(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            oProduct.NameCode = oProduct.NameCode == null ? "" : oProduct.NameCode;
            oProducts = Product.GetsByCodeOrName(oProduct, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        private bool ValidateInput(Product oProduct)
        {
            if (oProduct.ProductBaseID <= 0)
            {
                _sErrorMessage = "Please Insert The Base Product First";
                return false;
            }
            if (oProduct.ProductName == "")
            {
                _sErrorMessage = "Please Enter the Name";
                return false;
            }
            return true;
        }

        private ProductTree GetRoot()
        {
            ProductTree oProductTree = new ProductTree();
            foreach (ProductTree oItem in _oProductTrees)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oProductTree;
        }

        private void AddTreeNodes(ref ProductTree oProductTree)
        {
            List<ProductTree> oChildNodes;
            oChildNodes = GetChild(oProductTree.Objectid);
            oProductTree.children = oChildNodes;

            foreach (ProductTree oItem in oChildNodes)
            {
                ProductTree oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private List<ProductTree> GetChild(int nObjectID)
        {
            List<ProductTree> oProductTrees = new List<ProductTree>();
            foreach (ProductTree oItem in _oProductTrees)
            {
                if (oItem.parentid == nObjectID)
                {
                    oProductTrees.Add(oItem);
                }
            }
            return oProductTrees;
        }

        public void GetBaseProduct(ref ProductTree oProductTree)
        {
            if (oProductTree.IsLastLayer)
            {
                oProductTree.children.AddRange(GetChildBaseProduct(oProductTree.Objectid, oProductTree.id));
            }

            foreach (ProductTree oItem in oProductTree.children)
            {
                ProductTree oTemp = oItem;
                GetBaseProduct(ref oTemp);
            }
        }

        public List<ProductTree> GetChildBaseProduct(int nProductCategoryID, int nParentID)
        {
            List<ProductTree> oProductTrees = new List<ProductTree>();

            foreach (Product oItem in _oProducts)
            {
                if (oItem.ProductCategoryID == nProductCategoryID)
                {
                    i++;
                    ProductTree oTempProductTree = new ProductTree();
                    oTempProductTree = new ProductTree();
                    oTempProductTree.id = i;
                    oTempProductTree.text = oItem.ProductName;
                    oTempProductTree.state = "";
                    oTempProductTree.attributes = "";
                    oTempProductTree.parentid = nParentID;
                    oTempProductTree.ObjectType = "ProductCategory";
                    oTempProductTree.Objectid = oItem.ProductID;
                    oTempProductTree.IsLastLayer = false;
                    oProductTrees.Add(oTempProductTree);
                }
            }
            return oProductTrees;
        }



        public JsonResult setIDs(string ids)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, ids);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize("");
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        public JsonResult ProductGroupChangeSave(Product oProduct)
        {
            string productIds = oProduct.ProductName;
            int ProductCategoryID = oProduct.ProductCategoryID;
            int ProductBaseID = oProduct.ProductBaseID;

            

            List<Product> oProducts = new List<Product>();
            try
            {
                oProducts = Product.ProductGroupChangeSave(productIds, ProductCategoryID, ProductBaseID, (int)Session[SessionInfo.currentUserID]);
                
            }
            catch (Exception ex)
            {
                oProducts = new List<Product>();
                _oProduct = new Product();
                oProducts.Add(_oProduct);
                oProducts[0].ErrorMessage = ex.Message;
            }
             


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Actions
        public ActionResult MakeProductTree(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductTree = new ProductTree();
            _oProductTrees = new List<ProductTree>();


            #region Product Category
            _oProductCategorys = new List<ProductCategory>();
            _oProductCategorys = ProductCategory.Gets((int)Session[SessionInfo.currentUserID]);
            _oProducts = new List<Product>();
            _oProducts = Product.Gets((int)Session[SessionInfo.currentUserID]);

            i = 1;
            ProductTree oTempProductTree = new ProductTree();
            foreach (ProductCategory oItem in _oProductCategorys)
            {
                i++;
                oTempProductTree = new ProductTree();
                oTempProductTree.id = i;
                oTempProductTree.text = oItem.ProductCategoryName;
                oTempProductTree.state = "";
                oTempProductTree.attributes = "";
                oTempProductTree.parentid = oItem.ParentCategoryID;
                oTempProductTree.ObjectType = "ProductCategory";
                oTempProductTree.Objectid = oItem.ProductCategoryID;
                oTempProductTree.IsLastLayer = oItem.IsLastLayer;
                _oProductTrees.Add(oTempProductTree);
            }
            _oProductTree = GetRoot();
            this.AddTreeNodes(ref _oProductTree);
            this.GetBaseProduct(ref _oProductTree);
            #endregion
            return View(_oProductTree);
        }

        [HttpPost]
        public JsonResult AddProductAlias(Product oProduct)
        {
            try
            {
                oProduct = oProduct.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = sjson = serializer.Serialize(oProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Product Search
     
        [HttpPost]
        public JsonResult Gets(ProductBase oProductBase)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = GetSQL(oProductBase);
                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oProducts = new List<Product>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
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

            if (!string.IsNullOrEmpty(oProductBase.ProductCategoryIDs))
            {
                sProductCategoryIDs = oProductBase.ProductCategoryIDs.Remove(oProductBase.ProductCategoryIDs.Length - 1, 1);
            }
            if (!string.IsNullOrEmpty(oProductBase.PropertyIDs))
            {
                oProductBase.PropertyIDs = oProductBase.PropertyIDs.Remove(oProductBase.PropertyIDs.Length - 1, 1);
            }
            if (!string.IsNullOrEmpty(oProductBase.PropertyValueIDs))
            {
                oProductBase.PropertyValueIDs = oProductBase.PropertyValueIDs.Remove(oProductBase.PropertyValueIDs.Length - 1, 1);
            }

            string sReturn1 = "select * from View_Product";
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

        public ActionResult ViewProductSearch(string sTemp, string pc, double ts)
        {
            List<Product> oProducts = new List<Product>();
            oProducts = Product.GetsbyName(sTemp, pc, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oProducts);
        }
        #endregion

        #region Generalized Products Entry

        #region Generalized Products For yearn
        public ActionResult ViewGeneralizedYearnProducts(int nid, double ts)
        {
            ProductCategory oPC = new ProductCategory();
            oPC = oPC.Get(nid, (int)Session[SessionInfo.currentUserID]);

            _oProduct = new Product();
            _oProduct.ProductCategoryID = nid;
            //_oProduct.IsActivate = true;
            _oProduct.ProductCategoryName = oPC.ProductCategoryName;
            string sSQL = "SELECT * FROM View_Product WHERE ProductBaseID IN (SELECT ProductBaseID FROM ProductBase WHERE ProductCategoryID = " + nid + ")";
            _oProduct.Products = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //_oProductBase.ProductBases = ProductBase.GetsByCategory(nid, (int)Session[SessionInfo.currentUserID]);            
            return PartialView(_oProduct);
        }

        public ActionResult ViewProductYearn(int id) //Product  ID
        {
            _oProduct = new Product();
            if (id > 0)
            {
                _oProduct = _oProduct.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            //_oProduct.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Weight, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oProduct);
        }
      #endregion


        #region Generalized Products For Accesories
        public ActionResult ViewGeneralizedAccesoriesProducts(int nid, double ts)
        {
            ProductCategory oPC = new ProductCategory();
            oPC = oPC.Get(nid, (int)Session[SessionInfo.currentUserID]);
            _oProduct = new Product();
            _oProduct.ProductCategoryID = nid;
            //_oProduct.IsActivate = true;
            _oProduct.ProductCategoryName = oPC.ProductCategoryName;
            string sSQL = "SELECT * FROM View_Product WHERE ProductBaseID IN (SELECT ProductBaseID FROM ProductBase WHERE ProductCategoryID = " + nid + ")";
            _oProduct.Products = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //_oProductBase.ProductBases = ProductBase.GetsByCategory(nid, (int)Session[SessionInfo.currentUserID]);            
            return PartialView(_oProduct);
        }
        public ActionResult ViewProductAccesories(int id) //Product  ID
        {
            _oProduct = new Product();
            if (id > 0)
            {
                _oProduct = _oProduct.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oProduct.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oProduct);
        }
        #endregion

        #region Delete
        [HttpGet]
        public JsonResult DeleteGeneralizeProduct(int id)
        {
            string sErrorMease = "";
            try
            {
                Product oProduct = new Product();
                sErrorMease = oProduct.DeleteGeneralizeProduct(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region
        public ActionResult ViewProductSort(int ProductID, int BUID)
        {
            _oProduct = new Product();
            ProductSort oProductSort = new ProductSort();
            List<DUDyeingType> oDyeingTypes = new List<DUDyeingType>();
            List<DUDyeingTypeMapping> oDUDyeingTypeMappings = new List<DUDyeingTypeMapping>();
            DUDyeingTypeMapping oDUDyeingTypeMapping = new DUDyeingTypeMapping();
            _oProduct = _oProduct.Get(ProductID, (int)Session[SessionInfo.currentUserID]);
            _oProduct.BUID = BUID;
            if (_oProduct.ProductID > 0 && BUID > 0)
            {
                oProductSort = oProductSort.GetBy(_oProduct.ProductID, (int)Session[SessionInfo.currentUserID]);
                if(oProductSort.ProductID_Bulk<=0)
                {
                    oProductSort.ProductID_Bulk = _oProduct.ProductID;
                    oProductSort.ProductNameBulk = _oProduct.ProductName;
                }
                
                
                string sSql = "select * from DUDyeingTypeMapping where ProductID = " + _oProduct.ProductID;
                oDUDyeingTypeMappings = DUDyeingTypeMapping.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                string sDyeingTypeInts = string.Join(",", oDUDyeingTypeMappings.Select(x => x.DyeingTypeInt).ToList());

               if (string.IsNullOrEmpty(sDyeingTypeInts))
                sDyeingTypeInts = "0";
                oDyeingTypes = DUDyeingType.Gets("SELECT * FROM DUDyeingType WHERE Activity=1 and DyeingType not in (" + sDyeingTypeInts + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (DUDyeingType oitem in oDyeingTypes)
                {
                    oDUDyeingTypeMapping = new DUDyeingTypeMapping();
                    oDUDyeingTypeMapping.DyeingTypeMappingID = 0;
                    oDUDyeingTypeMapping.DyeingType = oitem.DyeingType;
                    oDUDyeingTypeMapping.ProductID = _oProduct.ProductID;
                    oDUDyeingTypeMapping.DyeingTypeInt =(int)oitem.DyeingType;
                    oDUDyeingTypeMappings.Add(oDUDyeingTypeMapping);
                }
            }
            ViewBag.ProductSort = oProductSort;
            ViewBag.DUDyeingTypeMappings = oDUDyeingTypeMappings;
            return View(_oProduct);
        }
        [HttpPost]
        public JsonResult Save_PSort(ProductSort oProductSort)
        {
            oProductSort.RemoveNulls();
            try
            {
                oProductSort = oProductSort.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oProductSort = new ProductSort();
                oProductSort.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductSort);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ProductPropertyInformation

       

        public ActionResult ViewProductPropertyInformation(int ProductID, int BUID)
        {
            _oProductPropertyInformation = new ProductPropertyInformation();
            if(ProductID>0 && BUID>0)
            {
                _oProductPropertyInformation.ProductPropertyInformations = ESimSol.BusinessObjects.ProductPropertyInformation.Gets(ProductID, BUID, (int)Session[SessionInfo.currentUserID]);       
            }
            ViewBag.PropertyValues = PropertyValue.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.PropertyTypes = EnumObject.jGets(typeof(EnumPropertyType));
            _oProductPropertyInformation.ProductID = ProductID;
            _oProductPropertyInformation.BUID = BUID;
            return View(_oProductPropertyInformation);
        }
        [HttpPost]
        public JsonResult SaveProductPropertyInfo(ProductPropertyInformation oProductPropertyInformation)
        {
            _oProductPropertyInformation = new ProductPropertyInformation();
            try
            {
                _oProductPropertyInformation = oProductPropertyInformation.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductPropertyInformation = new ProductPropertyInformation();
                _oProductPropertyInformation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductPropertyInformation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProductPropertyInfo(ProductPropertyInformation oProductPropertyInformation)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oProductPropertyInformation.Delete(oProductPropertyInformation.ProductPropertyInfoID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Product Searching By Name  Added By Sagor On 10 Feb 2015
        public ActionResult ProductSearchingByName(double ts)
        {
            ProductBase oProductBase = new ProductBase();
            return PartialView(oProductBase);
        }

        [HttpGet]
        public JsonResult SearchByProductName(string sName, string sValue, double nts)
        {

            string sSQL = "";

            if (sName.Trim() != "")
            {
                sSQL = "Select * from VIEW_Product as P where ProductName Like '%" + sName + "%'";
            }
            if (sValue != "" && sName != "")
            {
                string sDBObjectName = sValue.Split('~')[0];
                int nTriggerParentsType = Convert.ToInt32(sValue.Split('~')[1]);
                int nOperationalEvent = Convert.ToInt32(sValue.Split('~')[2]);
                int nInOutType = Convert.ToInt32(sValue.Split('~')[3]);
                int nDirections = Convert.ToInt32(sValue.Split('~')[4]);
                int nStoreID = Convert.ToInt32(sValue.Split('~')[5]);
                int nMapStoreID = Convert.ToInt32(sValue.Split('~')[6]);
                sSQL = sSQL + " AND P.ProductCategoryID in ( select ProductCategoryID from [dbo].[fn_GetProductCategoryByMTR] ( '" + sDBObjectName + "', " + nInOutType + ",  " + nTriggerParentsType + ",  " + nOperationalEvent + ",  " + nDirections + ", " + nStoreID + ", " + nMapStoreID + ", " + (int)Session[SessionInfo.currentUserID] + ")) order by ProductCategoryID";
            }

            //if (sValue == "" && sName != "")
            //{
            //    sSQL = sSQL + " And DBUserID =" + (int)Session[SessionInfo.currentUserID];
            //}

            _oProducts = new List<Product>();
            _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Activity
        [HttpGet]
        public JsonResult CommitActivity(int id, bool IsActive)
        {
            Product oProduct = new Product();
            try
            {
                oProduct = oProduct.CommitActivity(id, IsActive, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Searching
        [HttpPost]
        public JsonResult SearchByProductNameCode(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                 if (String.IsNullOrEmpty( oProduct.ProductName) && String.IsNullOrEmpty( oProduct.ProductCode) )
                { throw new Exception("Please Enter Product Name/Code to Search."); }
               // if (oProduct.ProductName.Trim() == "" && oProduct.ProductCode.Trim() == "") { throw new Exception("Please Enter Product Name/Code to Search."); }
                string sSQL = "Select * from VIEW_Product Where NameCode Like '%" + oProduct.ProductName + "%'";
                _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
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
        [HttpPost]
        public JsonResult GetStockQuantity(Product oProduct)
        {
            double nStockQuantity = 0;
            if (oProduct.ProductID > 0 && oProduct.BUID > 0)
            {
                nStockQuantity = Product.GetStockQuantity(oProduct.BUID, oProduct.ProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(nStockQuantity);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchByProductBUWise(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                _oProducts = Product.GetsByBU(oProduct.BUID, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                if (_oProducts.Count() <= 0) throw new Exception("No product found.");
            }
            catch (Exception ex)
            {
                _oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

       [HttpPost]
        public JsonResult SearchProductByName(Product oProduct)
        {
            _oProducts = new List<Product>();
            string sSQL = "";
            try
            {
                if (!String.IsNullOrEmpty(oProduct.ProductName))
                {
                    sSQL = "SELECT * FROM View_Product  WHERE ProductName LIKE '%" + oProduct.ProductName + "%' Order By ProductID";
                    _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oProducts = new List<Product>();
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(oProduct);
            }
            var jsonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        [HttpPost]
        public JsonResult Gets_ByName(Product oProduct)
        {
            string sSQL = "select * from View_Product";
            string sReturn = "";
            if (!string.IsNullOrEmpty(oProduct.ProductName))
            {
                Global.TagSQL(ref sReturn);
                oProduct.ProductName = oProduct.ProductName.Trim();
                sReturn = sReturn + " ProductName like  '%" + oProduct.ProductName + "%'";
            }
            if (!string.IsNullOrEmpty(oProduct.ProductCategoryName))
            {
                Global.TagSQL(ref sReturn);
                oProduct.ProductCategoryName = oProduct.ProductCategoryName.Trim();
                sReturn = sReturn + " ProductCategoryName like  '%" + oProduct.ProductCategoryName + "%'";
            }
            if (!string.IsNullOrEmpty(oProduct.GroupName))
            {
                Global.TagSQL(ref sReturn);
                oProduct.GroupName = oProduct.GroupName.Trim();
                sReturn = sReturn + " GroupName like  '%" + oProduct.GroupName + "%'";
            }
            if ((oProduct.BUID > 0))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID = " + oProduct.BUID + ")";
            }

            sSQL = sSQL + "" + sReturn + " order by ProductCategoryName,ProductName";
            _oProducts = new List<Product>();
            _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize((object)_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetsDyesChemical(Product oProduct)
        {
            string sSQL = "Select * from View_Product";
            string sReturn = "";
            if (!string.IsNullOrEmpty(oProduct.ProductName))
            {
                Global.TagSQL(ref sReturn);
                oProduct.ProductName = oProduct.ProductName.Trim();
                sReturn = sReturn + " Activity=1 and (ProductName like  '%" + oProduct.ProductName + "%' Or ProductCode ='%" + oProduct.ProductName + "%') ";
            }
            if (!string.IsNullOrEmpty(oProduct.ProductCategoryName))
            {
                Global.TagSQL(ref sReturn);
                oProduct.ProductCategoryName = oProduct.ProductCategoryName.Trim();
                if (oProduct.ProductCategoryName == "Dyes")
                {
                    sReturn = sReturn + "Activity=1 and ProductCategoryID =25 or ProductCategoryID in(SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + 1 + "," + (int)EnumProductNature.DyesChemical + "))";
                }
                else if (oProduct.ProductCategoryName == "Chemicals")
                {
                    sReturn = sReturn + " Activity=1 and ProductCategoryID =26";
                }
                else
                {
                    sReturn = sReturn + "Activity=1 and ProductCategoryID In (25,26)";
                }
                
            }
           if(oProduct.ProductCategoryID>0)
           {
               Global.TagSQL(ref sReturn);
               sReturn = sReturn + " Activity=1 and ProductCategoryID =25 or ProductCategoryID in(SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategoryBU](" + 1 + "," + (int)EnumProductNature.DyesChemical + "))";
           }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Activity=1 and ProductCategoryID In (25,26)";
            }

            sSQL = sSQL + "" + sReturn;
            _oProducts = new List<Product>();
            _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Printing
        public void ExportToExcel(int buid, double ts)
        {
            int nRowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            _oProducts = new List<Product>();
            string sSQL = "SELECT * FROM View_Product WHERE ProductCategoryID IN (SELECT HH.ProductCategoryID FROM BUWiseProductCategory AS HH WHERE HH.BUID=" + buid.ToString() + ") ORDER BY ProductCategoryID, ProductBaseID, ProductID ASC";
            _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Product List");
                sheet.Name = "Products";                
                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 20; //Group Name
                sheet.Column(4).Width = 20; //Product Code
                sheet.Column(5).Width = 60; //Product Name
                sheet.Column(6).Width = 15; //Unit Name
                sheet.Column(7).Width = 15; //Unit Symbol
                sheet.Column(8).Width = 10; //ProductID
                sheet.Column(9).Width = 10; //ProductID
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

                #region Report Header
                sheet.Cells[nRowIndex, 2, nRowIndex, 9].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                sheet.Cells[nRowIndex, 2, nRowIndex, 9].Merge = true;
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product List"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;
                #endregion

                #region Column Header
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Group Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Unit Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Unit Symbol"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "ProductID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Activity"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
                List<ContractorAddress> oContractorAddresss = new List<ContractorAddress>();

                #region Report Data                
                int nCount = 0, nProductCategoryID = 0;
                foreach (Product oItem in _oProducts)
                {
                    if (nProductCategoryID != oItem.ProductCategoryID)
                    {
                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = "Product Category : " + oItem.ProductCategoryName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.UnderLine = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }

                    nCount++;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "@"; 
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.GroupName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.MUnitName; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.ProductID; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "@"; 
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.ActivityInString; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "@";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nProductCategoryID = oItem.ProductCategoryID;
                    nRowIndex++;

                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Products.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region Reorder Level
        public ActionResult ViewSetReorderLevel(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ProductStock oProductStock = new ProductStock();
            List<ProductStock> oProductStocks = new List<ProductStock>();
            //oProductStock.BUID = buid;
            //oProductStocks = ProductStock.Gets(oProductStock, (int)Session[SessionInfo.currentUserID]);
            return View(oProductStocks);
        }

        [HttpPost]
        public JsonResult GetsForReorderLevel(ProductStock oProductStock)
        {            
            List<ProductStock> oProductStocks = new List<ProductStock>();            
            oProductStocks = ProductStock.Gets(oProductStock, (int)Session[SessionInfo.currentUserID]);
           

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductStocks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetReorderLevel(ProductStock oProductStock)
        {
            oProductStock = oProductStock.SetReorderLevel((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductStock);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintReorderLevelSetUp(int pcid, int buid, double ts)
        {
            ProductStock oProductStock = new ProductStock();
            List<ProductStock> oProductStocks = new List<ProductStock>();
            oProductStock.BUID = buid;
            oProductStock.ProductCategoryID = pcid;
            oProductStocks = ProductStock.Gets(oProductStock, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptReorderLevelSetup oReport = new rptReorderLevelSetup();
            byte[] abytes = oReport.PrepareReport(oProductStocks, oCompany, false);
            return File(abytes, "application/pdf");
        }


        public ActionResult ViewReorderLevelReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            ProductStock oProductStock = new ProductStock();
            List<ProductStock> oProductStocks = new List<ProductStock>();            
            return View(oProductStocks);
        }

        [HttpPost]
        public JsonResult GetsForReorderLevelReport(ProductStock oProductStock)
        {
            List<ProductStock> oProductStocks = new List<ProductStock>();
            oProductStocks = ProductStock.Gets(oProductStock, (int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductStocks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintReorderLevelReport(int pcid, int buid, double ts)
        {
            ProductStock oProductStock = new ProductStock();
            List<ProductStock> oProductStocks = new List<ProductStock>();
            oProductStock.BUID = buid;
            oProductStock.ProductCategoryID = pcid;
            oProductStock.IsReport = true;
            oProductStocks = ProductStock.Gets(oProductStock, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptReorderLevelSetup oReport = new rptReorderLevelSetup();
            byte[] abytes = oReport.PrepareReport(oProductStocks, oCompany, true);
            return File(abytes, "application/pdf");
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
        #endregion

        #region Gets Product by BU, Moudle and Product uses wise
        #region Product Gets
        [HttpPost]
        public JsonResult GetProductsByName(ProductBase oProductBase)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = GetSQL(oProductBase);
                oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oProducts = new List<Product>();
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetProductsByBUModuleWithProductUse(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, (EnumModuleName)oProduct.ModuleNameInInt, (EnumProductUsages)oProduct.ProductUsagesInInt, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, (EnumModuleName)oProduct.ModuleNameInInt, (EnumProductUsages)oProduct.ProductUsagesInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        [HttpPost]
        public JsonResult GetProductsByNameAndBUID(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();

            int nBUID = Convert.ToInt32(oProduct.Params.Split('~')[0]);
            string sProductName = oProduct.Params.Split('~')[1];
            int nModuleNameInInt = Convert.ToInt32(oProduct.Params.Split('~')[2]);
            int nProductUsagesInInt = Convert.ToInt32(oProduct.Params.Split('~')[3]);

            try
            {
                oProducts = Product.Gets("SELECT TOP(200)* FROM View_Product WHERE ProductName LIKE '%" + sProductName + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (sProductName != null && sProductName != "")
                //{
                //    oProducts = Product.GetsPermittedProductByNameCode(nBUID, (EnumModuleName)nModuleNameInInt, (EnumProductUsages)nProductUsagesInInt, sProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
                //else
                //{
                //    oProducts = Product.GetsPermittedProduct(nBUID, (EnumModuleName)nModuleNameInInt, (EnumProductUsages)nProductUsagesInInt, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
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
        #endregion

        #region Gets Product For Fabric Pattern
        [HttpPost]
        public JsonResult GetsProductForFabricPattern(Product oProduct)
        {

            int nFabricID = int.Parse(oProduct.Params.Split('~')[0]);

            string sSQL = "Select * from VIEW_Product as P Where P.ProductID<>0 ";

            if (!string.IsNullOrEmpty(oProduct.ProductName))
                sSQL = sSQL + " and (P.ProductName Like '%" + oProduct.ProductName.Trim() + "%' OR ShortName Like '%" + oProduct.ProductName.Trim() + "%')";

            string sSQLWithLabdipProduct="", sSQLWithPermission="";
            if (nFabricID > 0)
                sSQLWithLabdipProduct = " P.ProductID In (Select ProductID FROM LabdipDetail Where LabDipID In (Select LabDipID from Labdip Where FabricID=" + nFabricID + "))";

            if (oProduct.Params.Split('~').Count() > 7)
            {
                string sDBObjectName = oProduct.Params.Split('~')[1];
                int nTriggerParentsType = Convert.ToInt32(oProduct.Params.Split('~')[2]);
                int nOperationalEvent = Convert.ToInt32(oProduct.Params.Split('~')[3]);
                int nInOutType = Convert.ToInt32(oProduct.Params.Split('~')[4]);
                int nDirections = Convert.ToInt32(oProduct.Params.Split('~')[5]);
                int nStoreID = Convert.ToInt32(oProduct.Params.Split('~')[6]);
                int nMapStoreID = Convert.ToInt32(oProduct.Params.Split('~')[7]);
                sSQLWithPermission = " P.ProductCategoryID in ( select ProductCategoryID from [dbo].[fn_GetProductCategoryByMTR] ( '" + sDBObjectName + "', " + nOperationalEvent + ",  " + nTriggerParentsType + ",  " + nInOutType + ",  " + nDirections + ", " + nStoreID + ", " + nMapStoreID + ", " + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }


            if (!string.IsNullOrEmpty(sSQLWithLabdipProduct) && !string.IsNullOrEmpty(sSQLWithPermission))
                sSQL += " And ( " + sSQLWithLabdipProduct + " OR " + sSQLWithPermission + " )";
            else if (!string.IsNullOrEmpty(sSQLWithLabdipProduct))
                sSQL += " OR " + sSQLWithLabdipProduct + "";
            else if (!string.IsNullOrEmpty(sSQLWithPermission))
                sSQL += " And " + sSQLWithPermission + " ";


            _oProducts = new List<Product>();
            _oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CategoryTransfer
        public ActionResult ViewProductCategoryTransfer(int BUID, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUID = BUID;
            return View(new Product());
        }
        public JsonResult CategoryTransfer(ProductCategory oProductCategory)
        {
            _oProductCategory = new ProductCategory();
            try
            {
                _oProductCategory = oProductCategory;
                // SOURCE :_oProductCategory.ParentCategoryID
                // DESTINATION :_oProductCategory.ProductCategoryID
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
        #endregion

        #region Product Merge
        public ActionResult ViewProduct_Merge(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<Product> oProducts = new List<Product>();
          //  oProducts = Product.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(oProducts);
        }
        [HttpPost]
        public JsonResult ProductMarge(Product oProduct)
        {
            _oProduct = new Product();
            try
            {

                _oProduct = oProduct.ProductMarge((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets_ByNameForProductMerge(Product oProduct)
        {
            string sSQL = "select top(500)* from View_Product";
            string sReturn = "";
            if (!string.IsNullOrEmpty(oProduct.ProductName))
            {
                Global.TagSQL(ref sReturn);
                oProduct.ProductName = oProduct.ProductName.Trim();
                sReturn = sReturn + " ProductName like  '%" + oProduct.ProductName + "%'";
            }
            if ((oProduct.BUID > 0))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID = "+ oProduct.BUID + ")";
            }
            if (oProduct.Activity)
            {
                Global.TagSQL(ref sReturn);

                sReturn = sReturn + " Activity = " + 1 ;
            }
            else
            {
                Global.TagSQL(ref sReturn);

                sReturn = sReturn + " Activity = " + 0 ;
            }
            sSQL = sSQL + "" + sReturn + " order by ProductName";
            _oProducts = new List<Product>();
            _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    
    }




 }
