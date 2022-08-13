using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;


namespace ESimSolFinancial.Controllers
{
    public class MenuController : Controller
    {
        #region Declaration
        Menu _oMenu = new Menu();
        List<Menu> _oMenus = new List<Menu>();
        TMenu _oTMenu = new TMenu();
        List<TMenu> _oTMenus = new List<TMenu>();
        

        #endregion

        #region Functions
        private Menu GetRoot()
        {
            Menu oMenu = new Menu();
            foreach (Menu oItem in _oMenus)
            {
                if (oItem.ParentID == 0)
                {
                    return oItem;
                }
            }
            return oMenu;
        }

        private IEnumerable<Menu> GetChild(int nMenuID)
        {
            List<Menu> oMenus = new List<Menu>();
            foreach (Menu oItem in _oMenus)
            {
                if (oItem.ParentID == nMenuID)
                {
                    oMenus.Add(oItem);
                }
            }
            return oMenus;
        }

        private void AddTreeNodes(ref Menu oMenu)
        {
            IEnumerable<Menu> oChildNodes;
            oChildNodes = GetChild(oMenu.MenuID);
            oMenu.ChildNodes = oChildNodes;

            foreach (Menu oItem in oChildNodes)
            {
                Menu oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }


        private IEnumerable<TMenu> GetChildren(int nAccountHeadID)
        {
            List<TMenu> oTMenus = new List<TMenu>();
            foreach (TMenu oItem in _oTMenus)
            {
                if (oItem.parentid == nAccountHeadID)
                {
                    oTMenus.Add(oItem);
                }
            }
            return oTMenus;
        }

        private void AddTreeNodes(ref TMenu oTMenu)
        {
            IEnumerable<TMenu> oChildNodes;
            oChildNodes = GetChildren(oTMenu.id);
            oTMenu.children = oChildNodes;

            foreach (TMenu oItem in oChildNodes)
            {
                TMenu oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private TMenu GetRoot(int nParentID)
        {
            TMenu oTMenu = new TMenu();
            foreach (TMenu oItem in _oTMenus)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTMenu;
        }
        #endregion

        #region Action
        public ActionResult ViewMenus(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Menu).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oMenus = new List<Menu>();
            _oMenu = new Menu();
            _oTMenu = new TMenu();
            _oTMenus = new List<TMenu>();
            try
            {
                _oMenus = Menu.Gets((int)EnumApplicationType.WebApplication,  (int)Session[SessionInfo.currentUserID]);
                foreach (Menu oItem in _oMenus)
                {

                    _oTMenu = new TMenu();
                    _oTMenu.id = oItem.MenuID;
                    _oTMenu.parentid = oItem.ParentID;
                    _oTMenu.text = oItem.MenuName;
                    _oTMenu.ControllerName = oItem.ControllerName;
                    _oTMenu.ActionName = oItem.ActionName;
                    _oTMenu.BUName = oItem.BUName;
                    _oTMenu.IsActive = oItem.IsActive;
                    _oTMenu.ActivityInString = (oItem.IsActive)?"Active":"InActive";
                    if (oItem.ControllerName == "aaa" && oItem.ActionName == "aaa")
                    {
                        _oTMenu.state = "closed";
                    }
                    _oTMenus.Add(_oTMenu);
                }
                _oTMenu = new TMenu();
                _oTMenu = GetRoot(0);
                this.AddTreeNodes(ref _oTMenu);
                ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
                ViewBag.ModuleNames = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x=>x.Value);
                return View(_oTMenu);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oTMenu);
            }
        }

 

        [HttpPost]
        public JsonResult GetMenuChilds(Menu oMenu)
        {
            _oMenus = new List<Menu>();
            try
            {
                Menu oTempMenu = new Menu();
                oTempMenu = oTempMenu.Get(oMenu.ParentID, (int)Session[SessionInfo.currentUserID]);
                oMenu.DisplayMessage = "Selected parent menu : " + oTempMenu.MenuName;
                string sSQL = "SELECT * FROM View_Menu WHERE ParentID = " + oMenu.ParentID + " Order By MenuSequence";
                oMenu.ChildNodes = Menu.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMenu = new Menu();
                oMenu.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMenu);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetMenu(Menu oMenu)
        {
            try
            {
                oMenu = oMenu.Get(oMenu.MenuID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMenu = new Menu();
                oMenu.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMenu);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(Menu oMenu)
        {
            _oMenu = new Menu();
            try
            {
                _oMenu = oMenu;
                _oMenu = _oMenu.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMenu = new Menu();
                _oMenu.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMenu);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Menu oMenu)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMenu.Delete(oMenu.MenuID, (int)Session[SessionInfo.currentUserID]);
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

        #region GetUserWiseMenuList
        [HttpGet]
        public JsonResult GetUserWiseMenuList(double ts)
        {
            _oMenus = new List<Menu>();
            try
            {
                string sSQL = "SELECT * FROM View_Menu  WHERE MenuID IN (SELECT MenuID FROM UserPermissionFinance WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ") Order By MenuSequence";
                _oMenus = Menu.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMenus = new List<Menu>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMenus);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult RefreshMenuSequence(Menu oMenu)
        {
            try
            {
                oMenu = oMenu.RefreshSequence((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMenu.DisplayMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMenu);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        
    }
}
