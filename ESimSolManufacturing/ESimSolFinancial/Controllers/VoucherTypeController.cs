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
    public class VoucherTypeController : Controller
    {

        #region Declaration
        VoucherType _oVoucherType = new VoucherType();
        List<VoucherType> _oVoucherTypes = new List<VoucherType>();
        VoucherCode _oVoucherCode = new VoucherCode();
        List<VoucherCode> _oVoucherCodes = new List<VoucherCode>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewVoucherTypes(string gfdb, int menuid)
        {
            _oVoucherTypes = new List<VoucherType>();                        
            if (gfdb == null)
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.VoucherType).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
                _oVoucherTypes = VoucherType.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oVoucherTypes);
        }

        public ActionResult ViewVoucherType(int id)
        {
            _oVoucherType = new VoucherType();
            _oVoucherType.VoucherCodes = new List<VoucherCode>();
            if (id > 0)
            {
                _oVoucherType = _oVoucherType.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM VoucherCode WHERE VoucherTypeID=" + _oVoucherType.VoucherTypeID;
                _oVoucherType.VoucherCodes = VoucherCode.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oVoucherType);
        }

        [HttpPost]
        public JsonResult Save(VoucherType oVoucherType)
        {
            _oVoucherType = new VoucherType();

            try
            {
                oVoucherType.VoucherName = oVoucherType.VoucherName == null ? "" : oVoucherType.VoucherName;                
                oVoucherType.VoucherNumberFormat = oVoucherType.VoucherNumberFormat == null ? "" : oVoucherType.VoucherNumberFormat;
                oVoucherType.VoucherCodesInString = GetVoucherCodesInString(oVoucherType.VoucherCodes);
                oVoucherType.VoucherCategory = (EnumVoucherCategory)oVoucherType.VoucherCategoryInInt;
                oVoucherType.NumberMethod= (EnumNumberMethod)oVoucherType.NumberMethodInInt;

                _oVoucherType = oVoucherType.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oVoucherType = new VoucherType();
                _oVoucherType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oVoucherType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetVoucherCodesInString(List<VoucherCode> oVoucherCodes)
        {
            string sVoucherCodesInString = "";
            if (oVoucherCodes != null)
            {
                foreach (VoucherCode oItem in oVoucherCodes)
                {
                    sVoucherCodesInString = sVoucherCodesInString + oItem.VoucherCodeID.ToString() + "," + oItem.VoucherTypeID.ToString() + "," + (oItem.VoucherCodeTypeInInt).ToString() + "," + oItem.Value.ToString() + "," + oItem.Length.ToString() + "," + (oItem.RestartInInt).ToString() + "," + oItem.Sequence.ToString() + "~";
                }

                if (sVoucherCodesInString.Length > 0)
                {
                    sVoucherCodesInString = sVoucherCodesInString.Remove(sVoucherCodesInString.Length - 1, 1);
                }
            }
            return sVoucherCodesInString;
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                VoucherType oVoucherType = new VoucherType();
                sFeedBackMessage = oVoucherType.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets()
        {
            List<VoucherType> oVoucherTypes = new List<VoucherType>();
            VoucherType oVoucherType = new VoucherType();
            oVoucherType.VoucherName = "-- Select Voucher --";
            oVoucherTypes.Add(oVoucherType);
            oVoucherTypes.AddRange(VoucherType.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oVoucherTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       
    }
}
