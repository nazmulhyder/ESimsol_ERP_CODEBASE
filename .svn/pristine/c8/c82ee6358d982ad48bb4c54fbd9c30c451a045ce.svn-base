using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class MeasurementUnitController : Controller
    {
        //
        // GET: /MeasurementUnit/

        #region Declaration
        MeasurementUnit _oMeasurementUnit = new MeasurementUnit();
        UnitConversion _oUnitConversion = new UnitConversion();
        List<MeasurementUnit> _oMeasurementUnits = new List<MeasurementUnit>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private bool ValidateInput(MeasurementUnit oMeasurementUnit)
        {
            if (oMeasurementUnit.UnitName == null || oMeasurementUnit.UnitName == "")
            {
                _sErrorMessage = "Please enter Unit Name";
                return false;
            }
            if (oMeasurementUnit.UnitType == EnumUniteType.None)
            {
                _sErrorMessage = "Please select Unit Type";
                return false;
            }
            if (oMeasurementUnit.Symbol == null || oMeasurementUnit.Symbol == "")
            {
                _sErrorMessage = "Please enter Unit Symbol";
                return false;
            }
            return true;
        }

        //private bool ValidateInputForUnitConversion(UnitConversion oUnitConversion)
        //{
        //    if (oUnitConversion.ConvertedUnitValue == null || oUnitConversion.ConvertedUnitValue <= 0)
        //    {
        //        _sErrorMessage = "Please enter Converted Value Qty";
        //        return false;
        //    }
        //    return true;
        //}
        #endregion

        //Get Charts of Accounts list
        public ActionResult ViewMeasurementUnits(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
           // this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
          //  this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'MeasurementUnit','UnitConversion'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oMeasurementUnits = new List<MeasurementUnit>();
            _oMeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oMeasurementUnits);
        }


        //
        // Save: /ChartsOfAccount/
        public ActionResult ViewMeasurementUnit(int id)
        {
            if (id > 0)
            {
                _oMeasurementUnit = _oMeasurementUnit.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oMeasurementUnit = new MeasurementUnit();
            }
            return PartialView(_oMeasurementUnit);
        }

        [HttpPost]
        public JsonResult Save(MeasurementUnit oMeasurementUnit)
        {
            _oMeasurementUnit = new MeasurementUnit();
            try
            {
                _oMeasurementUnit.MeasurementUnitID = oMeasurementUnit.MeasurementUnitID;
                _oMeasurementUnit.UnitName = oMeasurementUnit.UnitName;
                _oMeasurementUnit.UnitType = (EnumUniteType)oMeasurementUnit.UnitTypeInt;
                _oMeasurementUnit.Symbol = oMeasurementUnit.Symbol;
                _oMeasurementUnit.Note = oMeasurementUnit.Note;
                _oMeasurementUnit.IsRound = oMeasurementUnit.IsRound;
                _oMeasurementUnit.IsSmallUnit = oMeasurementUnit.IsSmallUnit;
                _oMeasurementUnit = _oMeasurementUnit.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMeasurementUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeasurementUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                MeasurementUnit oMeasurementUnit = new MeasurementUnit();
                sErrorMease = oMeasurementUnit.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetbyUnitType(int etype, double ts)
        {
            List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
            string sSQL = "SELECT * FROM MeasurementUnit ORDER BY UnitType ASC";// +etype.ToString();
            oMeasurementUnits = MeasurementUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMeasurementUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetbyUnitType(MeasurementUnit oMeasurementUnit)
        {
            List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
            oMeasurementUnits = MeasurementUnit.Gets((EnumUniteType)oMeasurementUnit.UnitTypeInInt, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMeasurementUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult GetsMUnits(Product oProduct)
        {
            _oMeasurementUnits = MeasurementUnit.GetsbyProductID(oProduct.ProductID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oMeasurementUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Unit Conversion
        [HttpPost]
        public JsonResult GetsUCByReceipe(Recipe oRecipe)
        {
            List<UnitConversion> oUnitConversions = new List<UnitConversion>();           
            if (oRecipe.RecipeID > 0)
            {
                //string sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID IN (SELECT MM.ProductID FROM RecipeDetail AS MM WHERE MM.RecipeID=" + oRecipe.RecipeID.ToString() + ") ORDER BY HH.ProductID ASC";
                string sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID IN (SELECT ProductID FROM Product WHERE ProductBaseID IN (SELECT MM.ProductBaseID FROM View_RecipeDetail AS MM WHERE MM.RecipeID=" + oRecipe.RecipeID.ToString() + ")) ORDER BY HH.ProductID ASC";
                oUnitConversions = UnitConversion.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);               
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUnitConversions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsUCByProduct(Product oProduct)
        {
            List<UnitConversion> oUnitConversions = new List<UnitConversion>();
            if (oProduct.ProductID > 0)
            {
                string sSQL = "SELECT * FROM View_UnitConversion AS HH WHERE HH.ProductID = " + oProduct.ProductID.ToString() +" ORDER BY HH.ProductID ASC";
                oUnitConversions = UnitConversion.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUnitConversions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsExistingConversion(Product oProduct)
        {
            List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
            List<MeasurementUnit> oTempMeasurementUnits = new List<MeasurementUnit>();
            if (oProduct.ProductID > 0)
            {
                oProduct = oProduct.Get(oProduct.ProductID, (int)Session[SessionInfo.currentUserID]);
                oProduct.UnitConversions = UnitConversion.Gets(oProduct.ProductID, (int)Session[SessionInfo.currentUserID]);
                oMeasurementUnits = MeasurementUnit.Gets(oProduct.UnitType, (int)Session[SessionInfo.currentUserID]);
                foreach (MeasurementUnit oItem in oMeasurementUnits)
                {
                    if (oItem.MeasurementUnitID != oProduct.MeasurementUnitID)
                    {
                        oTempMeasurementUnits.Add(oItem);
                    }
                }
                oProduct.MeasurementUnits = oTempMeasurementUnits;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUnitConversion(UnitConversion oUnitConversion)
        {
            _oUnitConversion = new UnitConversion();
            try
            {
                _oUnitConversion = oUnitConversion.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oUnitConversion = new UnitConversion();
                _oUnitConversion.ErrorMessage = ex.Message.Split('~')[0];
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oUnitConversion);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteUnitConversion(UnitConversion oUnitConversion)
        {
            string sFeedBackMease = "";
            try
            {

                sFeedBackMease = oUnitConversion.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
