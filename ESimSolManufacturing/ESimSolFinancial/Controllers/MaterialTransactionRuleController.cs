using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ReportManagement;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class MaterialTransactionRuleController : Controller
    {
        #region Declaration
        MaterialTransactionRule _oMaterialTransactionRule = new MaterialTransactionRule();
        List<MaterialTransactionRule> _oMaterialTransactionRules = new List<MaterialTransactionRule>();
        List<ProductCategory> _oProductCategorys = new List<ProductCategory>();
        List<MaterialTransactionRule> oTriggerParents = new List<MaterialTransactionRule>();
        //List<OperationUnitContainingProductCategory> _oOperationUnitContainingProductCategorys = new List<OperationUnitContainingProductCategory>();
        TProductCategory _oTProductCategory = new TProductCategory();
        List<TProductCategory> _oTProductCategorys = new List<TProductCategory>();
        string _sErrorMessage = "";
        string _Ids = "";

        #endregion

        #region Functions
        private TProductCategory GetRoot1()
        {
            TProductCategory oTProductCategory = new TProductCategory();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return _oTProductCategory;
        }
        private IEnumerable<TProductCategory> GetChild1(int ParentID)
        {
            List<TProductCategory> oTProductCategorys = new List<TProductCategory>();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.parentid == ParentID)
                {
                    oTProductCategorys.Add(oItem);
                }
            }
            return oTProductCategorys;
        }

        private void AddTreeNodes1(ref TProductCategory oTProductCategory)
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

        private bool ValidateInput(MaterialTransactionRule oMaterialTransactionRule)
        {
            if (oMaterialTransactionRule.MaterialTransactionRuleID <= 0)
            {
                _sErrorMessage = "Please Select a Material Transaction Rule";
                return false;
            }
            return true;
        }
        #endregion

        
        #region Actions
        public ActionResult RefreshList(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            MaterialTransactionRule oMTR = new MaterialTransactionRule();
            return View(oMTR);
        }

        [HttpPost]
        public JsonResult Save(MaterialTransactionRule oMTR)
        {
            _oMaterialTransactionRule = new MaterialTransactionRule();
            try
            {
                _oMaterialTransactionRule.LocationID = oMTR.LocationID;
                _oMaterialTransactionRule.WorkingUnitID = oMTR.WorkingUnitID;
                _oMaterialTransactionRule.TriggerParentType = (EnumTriggerParentsType)oMTR.TriggerParentTypeInt;
                _oMaterialTransactionRule.InOutType = (EnumInOutType)oMTR.InOutTypeInt;
                _oMaterialTransactionRule.Direction = oMTR.Direction;
                _oMaterialTransactionRule.ProductCategoryID = oMTR.ProductCategoryID;
                _oMaterialTransactionRule.ProductType = (EnumProductType)oMTR.ProductTypeInt;
                _oMaterialTransactionRule.Note = oMTR.Note;



                _oMaterialTransactionRule = _oMaterialTransactionRule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMaterialTransactionRule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMaterialTransactionRule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Gets(string sIDs)
        {
            string IDs = "";
            IDs = sIDs;
            string sSQL = "";
            string sLocationIDs = Convert.ToString(IDs.Split('~')[0]);
            string sWorkingUnitIDs = Convert.ToString(IDs.Split('~')[1]);
            string sTPs = Convert.ToString(IDs.Split('~')[2]);
            string sIOs = Convert.ToString(IDs.Split('~')[3]);
            string sDirections = Convert.ToString(IDs.Split('~')[4]);
            sSQL = "SELECT * FROM View_MaterialTransactionRule WHERE LocationID IN (" + sLocationIDs + ") AND WorkingUnitID IN (" + sWorkingUnitIDs + ") AND TriggerParentType IN (" + sTPs + ") ";
            if (sIOs != "")
            {
                sSQL = sSQL + " AND InOutType IN (" + sIOs + ")";
            }
            if (sDirections != "")
            {
                sSQL = sSQL + " AND Direction IN (" + sDirections + ")";
            }
            List<MaterialTransactionRule> oMaterialTransactionRules = new List<MaterialTransactionRule>();
            try
            {
                oMaterialTransactionRules = (MaterialTransactionRule.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID));
            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMaterialTransactionRules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsPerSelection(string sIDs)
        {
            string IDs = "";
            IDs = sIDs;
            string sSQL = "";
            int WorkingUnitID = Convert.ToInt16(IDs.Split('~')[0]);
            int TriggerParentType = Convert.ToInt16(IDs.Split('~')[1]);
            int InOutType = Convert.ToInt16(IDs.Split('~')[2]);
            int Direction = Convert.ToInt16(IDs.Split('~')[3]);
            int ProductCategoryID = Convert.ToInt32(IDs.Split('~')[4]);
            int MaterialTransactionRuleID = Convert.ToInt16(IDs.Split('~')[5]);
            if (InOutType == 101)
            {
                InOutType = 102;
            }
            else
            {
                InOutType = 101;
            }

            sSQL = "SELECT  distinct VM.*,   CASE WHEN MTDR.MaterialTransactionRuleID= " + MaterialTransactionRuleID + " THEN 'Ase' WHEN MTDR.MaterialTransactionRuleID!= " + MaterialTransactionRuleID + " THEN 'Nai' END AS MAP FROM  View_MaterialTransactionRule VM LEFT JOIN MaterialTransactionDirectionMapping MTDR ON VM.MaterialTransactionRuleID = MTDR.MappingRuleID WHERE VM.InOutType = " + InOutType + " and VM.TriggerParentType = " + TriggerParentType + " and VM.Direction = 1 and VM.ProductCategoryID = " + ProductCategoryID + "and VM.WorkingUnitID != " + WorkingUnitID + " And VM.MaterialTransactionRuleID != " + MaterialTransactionRuleID + "Order BY LocationID";
            
            List<MaterialTransactionRule> oMaterialTransactionRules = new List<MaterialTransactionRule>();
            try
            {
                oMaterialTransactionRules = (MaterialTransactionRule.GetsForBiDirectionalRule(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID));
            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMaterialTransactionRules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsPerOtherSelection(string sIDs)
        {
            string IDs = "";
            IDs = sIDs;
            string sSQL = "";
            int WorkingUnitID = Convert.ToInt16(IDs.Split('~')[0]);
            int TriggerParentType = Convert.ToInt16(IDs.Split('~')[1]);
            int InOutType = Convert.ToInt16(IDs.Split('~')[2]);
            int Direction = Convert.ToInt16(IDs.Split('~')[3]);
            int ProductCategoryID = Convert.ToInt32(IDs.Split('~')[4]);
            int MaterialTransactionRuleID = Convert.ToInt32(IDs.Split('~')[5]);
            if (InOutType == 101)
            {
                InOutType = 102;
            }
            else
            {
                InOutType = 101;
            }

            sSQL = "SELECT  distinct VM.*,   CASE WHEN MTDR.MaterialTransactionRuleID= " + MaterialTransactionRuleID + " THEN 'Ase' WHEN MTDR.MaterialTransactionRuleID!= " + MaterialTransactionRuleID + " THEN 'Nai' END AS MAP FROM  View_MaterialTransactionRule VM LEFT JOIN MaterialTransactionDirectionMapping MTDR ON VM.MaterialTransactionRuleID = MTDR.MappingRuleID WHERE VM.InOutType = " + InOutType + " and VM.TriggerParentType = " + TriggerParentType + " and VM.Direction = 1 and VM.WorkingUnitID != " + WorkingUnitID + " And VM.MaterialTransactionRuleID != " + MaterialTransactionRuleID + "Order BY LocationID";

            List<MaterialTransactionRule> oMaterialTransactionRules = new List<MaterialTransactionRule>();
            try
            {
                oMaterialTransactionRules = (MaterialTransactionRule.GetsForBiDirectionalRule(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID));
            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMaterialTransactionRules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Activation(MaterialTransactionRule oMTR)
        {
            _oMaterialTransactionRule = new MaterialTransactionRule();
            try
            {
                _oMaterialTransactionRule.MaterialTransactionRuleID = oMTR.MaterialTransactionRuleID;
                _oMaterialTransactionRule.LocationID = oMTR.LocationID;
                _oMaterialTransactionRule.WorkingUnitID = oMTR.WorkingUnitID;
                _oMaterialTransactionRule.TriggerParentType = (EnumTriggerParentsType)oMTR.TriggerParentTypeInt;
                _oMaterialTransactionRule.InOutType = (EnumInOutType)oMTR.InOutTypeInt;
                _oMaterialTransactionRule.Direction = oMTR.Direction;
                _oMaterialTransactionRule.ProductCategoryID = oMTR.ProductCategoryID;
                _oMaterialTransactionRule.ProductType = (EnumProductType)oMTR.ProductTypeInt;
                _oMaterialTransactionRule.Note = oMTR.Note;
                _oMaterialTransactionRule.IsActive = oMTR.IsActive;

                if (_oMaterialTransactionRule.IsActive == true)
                {
                    _oMaterialTransactionRule.IsActive = false;
                    _oMaterialTransactionRule = _oMaterialTransactionRule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oMaterialTransactionRule.IsActive = true;
                    _oMaterialTransactionRule = _oMaterialTransactionRule.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                }


               
            }
            catch (Exception ex)
            {
                _oMaterialTransactionRule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMaterialTransactionRule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SaveMAP(string IDs, int nMTRID)
        {
            string value = "";
            try
            {
                MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
                oMaterialTransactionRule.MaterialTransactionRuleID = nMTRID;
                oMaterialTransactionRule.IDs = IDs;
                value = oMaterialTransactionRule.SaveMAP(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                value = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(value);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                MaterialTransactionRule oMaterialTransactionRule = new MaterialTransactionRule();
                oMaterialTransactionRule.MaterialTransactionRuleID = id;
                sErrorMease = oMaterialTransactionRule.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult OperationUnitProductPikerWithCheckBox()
        //{
        //    OperationUnitContainingProductCategory oOperationUnitContainingProductCategory = new OperationUnitContainingProductCategory();
           
        //    return PartialView();
        //}

        //[HttpPost]
        //public JsonResult OperationUnitProduct(OperationUnitContainingProductCategory oOperationUnitContainingProductCategory)
        //{

        //    _oOperationUnitContainingProductCategorys = new List<OperationUnitContainingProductCategory>();
        //    string sSQL = "SELECT Distinct * FROM View_OperationUnitContainingProductCategory WHERE IsActive = 1 AND  OperationUnitID = (SELECT OperationUnitID FROM WorkingUnit WHERE WorkingUnitID = " + oOperationUnitContainingProductCategory.OperationUnitID + ")";
        //    _oOperationUnitContainingProductCategorys = OperationUnitContainingProductCategory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    for (int i = 0; i < _oOperationUnitContainingProductCategorys.Count; i++)
        //    {
        //        if (_Ids == "")
        //        {
        //            _Ids = _oOperationUnitContainingProductCategorys[i].ProductCategoryID + ",";
        //        }
        //        else
        //        {
        //            _Ids = _Ids + _oOperationUnitContainingProductCategorys[i].ProductCategoryID + ",";
        //        }
        //    }

            
        //    _oProductCategorys = new List<ProductCategory>();
        //    _oTProductCategory = new TProductCategory();
        //    _oTProductCategorys = new List<TProductCategory>();
        //    ProductCategory oProductCategory = new ProductCategory();
        //    string sTableName = "VIEW_ProductCategory";
        //    string sPrimaryKey = "ProductCategoryID";
        //    string sParentRowName = "ParentCategoryID";
        //    bool IsParent = true;
        //    bool IsChild = false;
        //    bool IsAll = true;



        //    _oProductCategorys = ProductCategory.GetsForTree(sTableName, sPrimaryKey, sParentRowName, IsParent, IsChild, _Ids, IsAll, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    foreach (ProductCategory oItem in _oProductCategorys)
        //    {

        //        _oTProductCategory = new TProductCategory();
        //        _oTProductCategory.id = oItem.ProductCategoryID;
        //        _oTProductCategory.parentid = oItem.ParentCategoryID;
        //        _oTProductCategory.text = oItem.ProductCategoryName;
        //        _oTProductCategory.attributes = oItem.IsLastLayer.ToString();
        //        _oTProductCategory.Description = oItem.Note;
        //        _oTProductCategory.IsLastLayer = oItem.IsLastLayer;

        //        _oTProductCategorys.Add(_oTProductCategory);
        //    }
        //    _oTProductCategory = new TProductCategory();
        //    _oTProductCategory = GetRoot();
        //    this.AddTreeNodes(ref _oTProductCategory);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize((object)_oTProductCategory);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        private TProductCategory GetRoot()
        {
            TProductCategory oTProductCategory = new TProductCategory();
            foreach (TProductCategory oItem in _oTProductCategorys)
            {
                if (oItem.parentid == 0)
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
        
        #region Gets

        [HttpGet]
        public JsonResult GetsLocation()
        {
            List<Location> oLocations = new List<Location>();
            Location oLocation = new Location();
            string sSQLLoc = "SELECT * FROM Location WHERE IsActive = 1 AND LocationID IN (SELECT LocationID FROM [View_WorkingUnit] WHERE IsActive = 1 )";
            oLocations.AddRange(Location.Gets(sSQLLoc,((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetByLocation(int LocationID)
        {
            //string sErrorMease = "";
            int id = LocationID;
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            try
            {
                
                oWorkingUnits = WorkingUnit.Gets(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _sErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsWorkstation()
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();

            string sSQLWU = "SELECT * FROM View_WorkingUnit WHERE IsActive = 1 ";
            oWorkingUnits = WorkingUnit.Gets(sSQLWU,((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public JsonResult GetProductCategoryByWorkstation(int WUID)
        //{

        //    List<OperationUnitContainingProductCategory> oOperationUnitContainingProductCategorys = new List<OperationUnitContainingProductCategory>();
        //    try
        //    {
        //        string sSQL = "SELECT Distinct * FROM View_OperationUnitContainingProductCategory WHERE IsActive = 1 AND OperationUnitID = (SELECT OperationUnitID FROM WorkingUnit WHERE WorkingUnitID = " + WUID + ")";
        //        oOperationUnitContainingProductCategorys = OperationUnitContainingProductCategory.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _sErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oOperationUnitContainingProductCategorys);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult GetsTriggerParent()
        {
            List<TriggerParentObj> oTriggerParentObjs = new List<TriggerParentObj>();
            TriggerParentObj oTriggerParentObj = new TriggerParentObj();
            foreach (int oItem in Enum.GetValues(typeof(EnumTriggerParentsType)))
            {
                if (oItem != 0)
                {
                    oTriggerParentObj = new TriggerParentObj();
                    oTriggerParentObj.id = oItem;
                    oTriggerParentObj.Name = ((EnumTriggerParentsType)oItem).ToString();
                    oTriggerParentObjs.Add(oTriggerParentObj);
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTriggerParentObjs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsDirection()
        {
            List<DirectionObj> oDirectionObjs = new List<DirectionObj>();
            DirectionObj oDirectionObj = new DirectionObj();
            foreach (int oItem in Enum.GetValues(typeof(EnumInOutType)))
            {
                if (oItem != 100)
                {
                    oDirectionObj = new DirectionObj();
                    oDirectionObj.id = oItem;
                    oDirectionObj.TypeName = ((EnumInOutType)oItem).ToString();
                    oDirectionObjs.Add(oDirectionObj);
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oDirectionObjs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class TriggerParentObj
    {
        public TriggerParentObj()
        {
            id = 0;
            Name = "";
        }
        public int id { get; set; }
        public string Name { get; set; }
    }
    public class DirectionObj
    {
        public DirectionObj()
        {
            id = 0;
            TypeName = "";
        }
        public int id { get; set; }
        public string TypeName { get; set; }
    }
}
