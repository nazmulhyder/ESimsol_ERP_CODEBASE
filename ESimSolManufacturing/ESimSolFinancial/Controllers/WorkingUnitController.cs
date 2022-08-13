using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class WorkingUnitController : Controller
    {
        #region New
        #region Declaration
        WorkingUnit _oWorkingUnit = new WorkingUnit();
        List<WorkingUnit> _oWorkingUnits = new List<WorkingUnit>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private bool ValidateInput(WorkingUnit oWorkingUnit)
        {
            if (oWorkingUnit.WorkingUnitCode == null || oWorkingUnit.WorkingUnitCode == "")
            {
                _sErrorMessage = "Please enter Working Unit Code";
                return false;
            }
            if (oWorkingUnit.OperationUnitID == null || oWorkingUnit.OperationUnitID <= 0)
            {
                _sErrorMessage = "Invalid Operation please try again";
                return false;
            }
            if (oWorkingUnit.LocationID == null || oWorkingUnit.LocationID <= 0)
            {
                _sErrorMessage = "Invalid Location please try again";
                return false;
            }
            return true;
        }
        #endregion

        public ActionResult RefreshList(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oWorkingUnits = new List<WorkingUnit>();
            _oWorkingUnits = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit WHERE BUID =" + buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oWorkingUnits);
        }

        public ActionResult Add()
        {
            _oWorkingUnit = new WorkingUnit();
            _oWorkingUnit.LocationList = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oWorkingUnit.OperationUnitList = OperationUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oWorkingUnit);
        }

        public ActionResult ViewWorkingUnit(int id)
        {
            _oWorkingUnit = new WorkingUnit();
            if (id>0)
            {
                _oWorkingUnit = _oWorkingUnit.Get(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oWorkingUnit.LocationList = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oWorkingUnit.OperationUnitList = OperationUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            List<EnumObject> oUnitTypeObjs = new List<EnumObject>();
            oUnitTypeObjs = EnumObject.jGets(typeof(EnumWoringUnitType));
            oUnitTypeObjs.Sort((x, y) => string.Compare(x.Value, y.Value));
            _oWorkingUnit.UnitTypeObjs = oUnitTypeObjs;
            return View(_oWorkingUnit);
        }

        public JsonResult Save(WorkingUnit oWorkingUnit)
        {
            _oWorkingUnit = new WorkingUnit();
            _oWorkingUnit = oWorkingUnit;
            try
            {                
                _oWorkingUnit = _oWorkingUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oWorkingUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkingUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(WorkingUnit oWorkingUnit)
        {
            try
            {
                if (this.ValidateInput(oWorkingUnit))
                {                    
                    oWorkingUnit = oWorkingUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    return RedirectToAction("RefreshList");
                }
                TempData["message"] = _sErrorMessage;
                return View(oWorkingUnit);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }

        public ActionResult Edit(int id)
        {
            _oWorkingUnit = new WorkingUnit();
            _oWorkingUnit = _oWorkingUnit.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oWorkingUnit.LocationList = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oWorkingUnit.OperationUnitList = OperationUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oWorkingUnit);
        }

        [HttpPost]
        public ActionResult Edit(WorkingUnit oWorkingUnit)
        {
            try
            {
                if (this.ValidateInput(oWorkingUnit))
                {                    
                    oWorkingUnit = oWorkingUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                    return RedirectToAction("RefreshList");
                }
                TempData["message"] = _sErrorMessage;
                return View(oWorkingUnit);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                _oWorkingUnit = new WorkingUnit();
                sErrorMease = _oWorkingUnit.Delete(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Details(int id)
        {
            _oWorkingUnit = new WorkingUnit();
            _oWorkingUnit = _oWorkingUnit.Get(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            _oWorkingUnit.LocationList = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oWorkingUnit.OperationUnitList = OperationUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oWorkingUnit);
        }

        #region Searching
        public ActionResult WorkingUnitSearch()
        {
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            return PartialView(oWorkingUnits);
        }


        [HttpGet]
        public JsonResult Get(int id)
        {
            WorkingUnit oWorkingUnit = new WorkingUnit();
            try
            {
                oWorkingUnit = oWorkingUnit.Get(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oWorkingUnit = new WorkingUnit();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkingUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            _oWorkingUnits = new List<WorkingUnit>();
            try
            {
                _oWorkingUnits = WorkingUnit.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oWorkingUnits = new List<WorkingUnit>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        #region Auto Configure
        public ActionResult View_WorkingUnit_AutoConfiguration(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<Location> _oLocations = new List<Location>();
            _oLocations = Location.GetsIncludingStore(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oLocations);
        }

        [HttpPost]
        public ActionResult WorkingUnit_AutoConfiguration(WorkingUnit oWorkingUnit)
        {
            string sErrorMease = "";
            try
            {
                //if (this.ValidateInput(oWorkingUnit))
                //{
                sErrorMease = oWorkingUnit.WorkingUnit_AutoConfiguration(oWorkingUnit.WorkingUnitID, oWorkingUnit.LocationID, 0, 0, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //}

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
        public ActionResult UserPermissionConfiguration(WorkingUnit oWorkingUnit)
        {
            string sErrorMease = "";
            try
            {
                //if (this.ValidateInput(oWorkingUnit))
                //{
                sErrorMease = oWorkingUnit.WorkingUnit_AutoConfiguration(0, 0, oWorkingUnit.WorkingUnitID, oWorkingUnit.LocationID, 5, ((User)Session[SessionInfo.CurrentUser]).UserID);

                //}

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
        public ActionResult GetLocationFor_AutoConfiguration()
        {
            string sMassage = "";
            List<Location> oLocations = new List<Location>();
            try
            {

                oLocations = Location.Gets("SELECT * FROM View_Location where ParentID !=0 and IsActive =1 and LocationID not in (Select WorkingUnit.LocationID from WorkingUnit)", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                sMassage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLocations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetsStore
        //[HttpPost]
        //public JsonResult GetsStoreForAccount()
        //{
        //    List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
        //    oWorkingUnits = WorkingUnit.Gets("SELECT WorkingUnit.*,OperationUnit.IsStore, OperationUnit.OperationUnitName, OperationUnit.ContainingProduct as ProductCategory, Location.[Name] as "
        //+ " LocationName from WorkingUnit, OperationUnit, Location where WorkingUnit.CompanyID=" + 1+" AND  (WorkingUnit.OperationUnitID = OperationUnit.OperationUnitID) AND (WorkingUnit.LocationID = " + "Location.LocationID) and Location.IsActive = 1 AND IsStore = 1 and OperationUnit.OperationUnitID in (2,4,7,8,22,23,24,25)", ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize((object)oWorkingUnits);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
       
        #endregion

        [HttpPost]
        public JsonResult GetsBUWtihModuleWise(WorkingUnit oWorkingUnit)
        {
            if (string.IsNullOrEmpty(oWorkingUnit.LocationName))
            {
                _oWorkingUnits = WorkingUnit.GetsPermittedStore(oWorkingUnit.BUID, (EnumModuleName)oWorkingUnit.ModuleName, (EnumStoreType)oWorkingUnit.StoreType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oWorkingUnits = WorkingUnit.GetsPermittedStoreByStoreName(oWorkingUnit.BUID, (EnumModuleName)oWorkingUnit.ModuleName, (EnumStoreType)oWorkingUnit.StoreType,oWorkingUnit.LocationName,  ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBUWiseWorkingUnit(WorkingUnit oWorkingUnit)
        {
            string sSQL = "SELECT * FROM View_WorkingUnit ";
            string sTempSql = "";
            if(oWorkingUnit.BUID!=0)
            {
                Global.TagSQL(ref sTempSql);
                sTempSql += " BUID = "+oWorkingUnit.BUID;
            }
            if (oWorkingUnit.LocationName != null)
            {
                Global.TagSQL(ref sTempSql);
                sTempSql += " LocationName LIKE '%" + oWorkingUnit.LocationName + "%'";
            } 
            if (!string.IsNullOrEmpty(oWorkingUnit.BUIDs))
            {
                Global.TagSQL(ref sTempSql);
                sTempSql += " BUID IN (" + oWorkingUnit.BUIDs+")";
            }
            sSQL += sTempSql;
            _oWorkingUnits = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateForAcitivity(WorkingUnit oWorkingUnit)
        {
            _oWorkingUnit = new WorkingUnit();
            string sErrorMease = "";
            try
            {
                oWorkingUnit.IsActive = (oWorkingUnit.IsActive == true ? false : true);
                _oWorkingUnit = oWorkingUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWorkingUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsWorkingUnit(WorkingUnit oWorkingUnit)
        {
            oWorkingUnit.LOUNameCode = oWorkingUnit.LOUNameCode == null ? "" : oWorkingUnit.LOUNameCode;
            string sLOUNameCode = oWorkingUnit.LOUNameCode.Split('[')[0];
            _oWorkingUnits = WorkingUnit.GetsbyName(sLOUNameCode, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oWorkingUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
