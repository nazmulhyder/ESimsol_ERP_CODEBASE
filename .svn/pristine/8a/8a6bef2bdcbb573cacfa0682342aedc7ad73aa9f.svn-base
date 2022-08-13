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
    public class GarmentsClassController :  Controller
    {
        #region Declaration
        GarmentsClass _oGarmentsClass = new GarmentsClass();
        TGarmentsClass _oTGarmentsClass = new TGarmentsClass();

        List<GarmentsClass> _oGarmentsClasss = new List<GarmentsClass>();
        List<TGarmentsClass> _oTGarmentsClasss = new List<TGarmentsClass>();      
        string _sErrorMessage = "";
        #endregion



        #region Function
        private TGarmentsClass GetRoot()
        {
            TGarmentsClass oTGarmentsClass = new TGarmentsClass();
            foreach (TGarmentsClass oItem in _oTGarmentsClasss)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return _oTGarmentsClass;
        }

        private void AddTreeNodes(ref TGarmentsClass oTGarmentsClass)
        {
            IEnumerable<TGarmentsClass> oChildNodes;
            oChildNodes = GetChild(oTGarmentsClass.id);
            oTGarmentsClass.children = oChildNodes;

            foreach (TGarmentsClass oItem in oChildNodes)
            {
                TGarmentsClass oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private IEnumerable<TGarmentsClass> GetChild(int nParentCategoryID)
        {
            List<TGarmentsClass> oTGarmentsClasss = new List<TGarmentsClass>();
            foreach (TGarmentsClass oItem in _oTGarmentsClasss)
            {
                if (oItem.parentid == nParentCategoryID)
                {
                    oTGarmentsClasss.Add(oItem);
                }
            }
            return oTGarmentsClasss;
        }
        #endregion

        public ActionResult ViewGarmentsClasss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GarmentsClass).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oGarmentsClasss = new List<GarmentsClass>();
            _oGarmentsClass = new GarmentsClass();
            _oTGarmentsClass = new TGarmentsClass();
            _oTGarmentsClasss = new List<TGarmentsClass>();            
            try
            {
                _oGarmentsClasss = GarmentsClass.Gets((int)Session[SessionInfo.currentUserID]);
                foreach (GarmentsClass oItem in _oGarmentsClasss)
                {

                    _oTGarmentsClass = new TGarmentsClass();
                    _oTGarmentsClass.id = oItem.GarmentsClassID;
                    _oTGarmentsClass.text = oItem.ClassName;
                    _oTGarmentsClass.state = "";
                    _oTGarmentsClass.attributes = "";
                    _oTGarmentsClass.parentid = oItem.ParentClassID;
                    _oTGarmentsClass.Description = oItem.Note;                    
                    _oTGarmentsClasss.Add(_oTGarmentsClass);
                }
                _oTGarmentsClass = new TGarmentsClass();
                _oTGarmentsClass = GetRoot();
                this.AddTreeNodes(ref _oTGarmentsClass);
                return View(_oTGarmentsClass);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTGarmentsClass);
            }
        }


        public ActionResult ViewGarmentsClass(int id)
        {
            GarmentsClass oGarmentsClass = new GarmentsClass();
            oGarmentsClass.ParentClassID = id;
            return View(oGarmentsClass);
        }


        [HttpPost]
        public JsonResult Save(GarmentsClass oGarmentsClass)
        {
            _oGarmentsClass = new GarmentsClass();
            try
            {
                _oGarmentsClass = oGarmentsClass;
                _oGarmentsClass = _oGarmentsClass.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGarmentsClass = new GarmentsClass();
                _oGarmentsClass.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGarmentsClass);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult getchildren(int parentid)
        {
            _oGarmentsClasss = new List<GarmentsClass>();
            _oTGarmentsClass = new TGarmentsClass();
            _oTGarmentsClasss = new List<TGarmentsClass>();
            try
            {
                string sSQL = "SELECT * FROM GarmentsClass WHERE ParentClassID=" + parentid.ToString();
                _oGarmentsClasss = GarmentsClass.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oGarmentsClass = new GarmentsClass();
                _oGarmentsClass.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGarmentsClasss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sfeedbackmessage = "";
            _oGarmentsClass = new GarmentsClass();
            GarmentsClass oGarmentsClass = new GarmentsClass();
            try
            {
                sfeedbackmessage = _oGarmentsClass.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sfeedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGarmentsClass(int id)
        {
            _oGarmentsClass = new GarmentsClass();
            _oGarmentsClass = _oGarmentsClass.Get(id, (int)Session[SessionInfo.currentUserID]);        
            return View(_oGarmentsClass);
        }
    }    
}
