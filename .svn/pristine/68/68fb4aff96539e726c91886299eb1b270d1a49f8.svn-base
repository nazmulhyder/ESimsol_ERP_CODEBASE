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
using ICS.Core.Utility;
using System.Linq;

namespace ESimSolFinancial.Controllers
{
    public class AuthorizationRoleController : Controller
    {

        #region Declartion
        AuthorizationRole _oAuthorizationRole = new AuthorizationRole();
        List<AuthorizationRole> _oAuthorizationRoles = new List<AuthorizationRole>();
        AuthorizationRoleMapping _oAuthorizationRoleMapping = new AuthorizationRoleMapping();
        List<AuthorizationRoleMapping> _oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();

        #endregion

        #region Refresh List
        public ActionResult ViewAuthorizationRoles(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AuthorizationRole).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            
            _oAuthorizationRoles = new List<AuthorizationRole>();
            _oAuthorizationRoles = AuthorizationRole.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            _oAuthorizationRoles.Sort((x, y) => string.Compare(x.RoleName, y.RoleName));
            return View(_oAuthorizationRoles);
        }
        #endregion

        #region Add, Edit, Delete

        #region View Add Authorization Role
        public ActionResult ViewAuthorizationRole(int nId)
        {
            _oAuthorizationRole = new AuthorizationRole();
            List<EnumObject> oModuleNameObjs = new List<EnumObject>();
            oModuleNameObjs = EnumObject.jGets(typeof(EnumModuleName));
            oModuleNameObjs.Sort((x, y) => string.Compare(x.Value, y.Value));
            //_oAuthorizationRole = _oAuthorizationRole.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oAuthorizationRole.ModuleNameObjs = oModuleNameObjs;
             ViewBag.RoleOperationTypeObjs = EnumObject.jGets(typeof(EnumRoleOperationType)).OrderBy(x => x.Value);
            return View(_oAuthorizationRole);
        }
        //public ActionResult ViewAddAuthorizationRole(double ts)
        //{
        //    _oAuthorizationRole = new AuthorizationRole();
        //    List<EnumObject> oModuleNameObjs = new List<EnumObject>();
        //    oModuleNameObjs = EnumObject.jGets(typeof(EnumModuleName));
        //    oModuleNameObjs.Sort((x, y) => string.Compare(x.Value, y.Value));
        //    _oAuthorizationRole.ModuleNameObjs = oModuleNameObjs;
        //    return PartialView(_oAuthorizationRole);
        //}
        #endregion

        #region View Add Authorization Role
        public ActionResult ViewAuthorizationRoleEdit(int id)
        {
            _oAuthorizationRole = new AuthorizationRole();
            _oAuthorizationRole = _oAuthorizationRole.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<EnumObject> oModuleNameObjs = new List<EnumObject>();
            oModuleNameObjs = EnumObject.jGets(typeof(EnumModuleName));
            oModuleNameObjs.Sort((x, y) => string.Compare(x.Value, y.Value));
            _oAuthorizationRole.ModuleNameObjs = oModuleNameObjs;
            _oAuthorizationRole.RoleOperationTypeObjs = EnumObject.jGets(typeof(EnumRoleOperationType));
            return PartialView(_oAuthorizationRole);
        }
        #endregion

        #region Operation picker
        public ActionResult OperationPicker(double ts)
        {
            AuthorizationRole oAuthorizationRole = new AuthorizationRole();
            oAuthorizationRole.RoleOperationTypeObjs = EnumObject.jGets(typeof(EnumRoleOperationType));
            return PartialView(oAuthorizationRole);
        }

        //GetOperationList
        [HttpGet]
        public JsonResult GetOperationList()
        {
            List<EnumRoleOPeration> oEnumRoleOPerations = new List<EnumRoleOPeration>();
            EnumRoleOPeration oEnumRoleOPeration = new EnumRoleOPeration();
            try
            {
                foreach (int oItem in Enum.GetValues(typeof(EnumRoleOperationType)))
                {

                    if (oItem != 0)
                    {
                        oEnumRoleOPeration = new EnumRoleOPeration();
                        oEnumRoleOPeration.id = oItem;
                        oEnumRoleOPeration.Value = ((EnumRoleOperationType)oItem).ToString();
                        oEnumRoleOPerations.Add(oEnumRoleOPeration);
                    }

                }

            }
            catch (Exception ex)
            {
                //smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEnumRoleOPerations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(AuthorizationRole oAuthorizationRole)
        {
            _oAuthorizationRoles = new  List<AuthorizationRole>();
            try
            {                
                _oAuthorizationRoles = oAuthorizationRole.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oAuthorizationRole = new AuthorizationRole();
                _oAuthorizationRole.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAuthorizationRoles);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string feedbackmessage = "";
            try
            {
                AuthorizationRole oAuthorizationRole = new AuthorizationRole();
                feedbackmessage = oAuthorizationRole.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                feedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(feedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Authorization Role mapping
        public ActionResult ViewAssignToUser(int id, double ts) // RoleID
        {
             _oAuthorizationRoleMapping = new AuthorizationRoleMapping();
             if (id > 0)
             {
                 _oAuthorizationRoleMapping.AuthorizationRoleMappings = AuthorizationRoleMapping.GetsByRole(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
             }
            TempData["AuthorzationRoleID"]=id;
            _oAuthorizationRoleMapping.Users = ESimSol.BusinessObjects.User.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oAuthorizationRoleMapping);
        }

        public ActionResult ViewRoleAssign(int id, double ts) // UserID 
        {
            _oAuthorizationRoles = new List<AuthorizationRole>();
            ESimSol.BusinessObjects.User oSelectedUser = new ESimSol.BusinessObjects.User();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            if (id > 0)
            {
                oSelectedUser = oSelectedUser.Get(id, (int)Session[SessionInfo.currentUserID]);
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.SelectedUser = oSelectedUser;
            ViewBag.AuthorizationRoleMappings = oAuthorizationRoleMappings;
            _oAuthorizationRoles = AuthorizationRole.Gets((int)Session[SessionInfo.currentUserID]);
            _oAuthorizationRoles = _oAuthorizationRoles.OrderBy(x => x.ModuleNameST).ThenBy(x => x.OperationTypeInt).ToList();
            return View(_oAuthorizationRoles);
        }

        public ActionResult ViewRoleDisallow(int id, double ts) // UserID 
        {            
            ESimSol.BusinessObjects.User oSelectedUser = new ESimSol.BusinessObjects.User();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            if (id > 0)
            {
                oSelectedUser = oSelectedUser.Get(id, (int)Session[SessionInfo.currentUserID]);
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByUser(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.SelectedUser = oSelectedUser;            
            return View(oAuthorizationRoleMappings);
        }

        #region Commit Mapping Role
        [HttpPost]
        public JsonResult CommitMappingRole(AuthorizationRoleMapping oAuthorizationRoleMapping)
        {
            string sfeedBackMessage = "";
            try
            {
                sfeedBackMessage = oAuthorizationRoleMapping.Save(oAuthorizationRoleMapping.IsShortList, oAuthorizationRoleMapping.IsUserBased, (int)Session[SessionInfo.currentUserID]).ToString();
            }
            catch (Exception ex)
            {
                _oAuthorizationRole = new AuthorizationRole();
                _oAuthorizationRole.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisallowMappingRole(AuthorizationRoleMapping oAuthorizationRoleMapping)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            try
            {
                oAuthorizationRoleMappings = AuthorizationRoleMapping.DisallowMappingRole(oAuthorizationRoleMapping, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAuthorizationRoleMapping = new AuthorizationRoleMapping();
                oAuthorizationRoleMapping.ErrorMessage = ex.Message;
                oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
                oAuthorizationRoleMappings.Add(oAuthorizationRoleMapping);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAuthorizationRoleMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region Commit Authoization
        [HttpGet]
        public JsonResult CommitAuthorization(int id, int nAssignUserID, string sPermissions)
        {
            _oAuthorizationRole = new AuthorizationRole();
            string sFeedbackMessage = "";
            try
            {
                if (id > 0 && nAssignUserID > 0)
                {
                    int nCount = 0;
                    bool bUserPermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bAuthorizationRule = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bStorePermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bProductPermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bBUPermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bTimeCardPermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bAutoVoucharPermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);
                    bool bDashBoardPermission = Convert.ToBoolean(sPermissions.Split('~')[nCount++]);

                    sFeedbackMessage = _oAuthorizationRole.CopyAuthorization(nAssignUserID, id, bUserPermission, bAuthorizationRule, bStorePermission, bProductPermission, bBUPermission, bTimeCardPermission, bAutoVoucharPermission, bDashBoardPermission,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch(Exception e)
            {
                sFeedbackMessage = e.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedbackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

    #region Role Operation Object
    public class EnumRoleOPeration
    {
        public EnumRoleOPeration()
        {
            id = 0;
            Value = "";
        }
      
        
       public int id { get; set; }
       public string Value { get; set; }
    }


    #endregion
}
