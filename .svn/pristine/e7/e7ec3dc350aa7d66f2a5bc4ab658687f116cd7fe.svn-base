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
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class AccountEffectController : Controller
    {
        #region Declaration
        AccountEffect _oAccountEffect = new AccountEffect();
        List<AccountEffect> _oAccountEffects = new List<AccountEffect>();
        #endregion
                
        public ActionResult ViewAccountEffect(int mid, int mtype, double ts)
        {    
            //mid = moudle name obj ID
            //mtype = module name
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountEffect).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oAccountEffect = new AccountEffect();
            _oAccountEffect.AccountEffects = AccountEffect.Gets(mid, (EnumModuleName)mtype, (int)Session[SessionInfo.currentUserID]);
            _oAccountEffect.AccountEffectTypes = EnumObject.jGets(typeof(EnumAccountEffectType));
            _oAccountEffect.ModuleName = (EnumModuleName)mtype;
            _oAccountEffect.ModuleNameInt = mtype;
            _oAccountEffect.ModuleObjID = mid;            
            return View(_oAccountEffect);
        }
             
        [HttpPost]
        public JsonResult SaveAccountEffect(AccountEffect oAccountEffect)
        {
            _oAccountEffect = new AccountEffect();            
            try
            {
                _oAccountEffect = oAccountEffect;
                if (_oAccountEffect.Remarks == null) { _oAccountEffect.Remarks = ""; }
                _oAccountEffect = _oAccountEffect.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oAccountEffect = new AccountEffect();
                _oAccountEffect.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountEffect);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAccountEffect(AccountEffect oAccountEffect)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oAccountEffect.Delete(oAccountEffect.AccountEffectID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
