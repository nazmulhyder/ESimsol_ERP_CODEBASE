using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace ESimSol.Controllers
{
    public class SalarySheetPropertyController : Controller
    {
        #region Declaration
        SalarySheetProperty _oSalarySheetProperty = new SalarySheetProperty();
        List<SalarySheetProperty> _oSalarySheetPropertys = new List<SalarySheetProperty>();
        #endregion

        #region Salary Sheet Property
        private static Int16 GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());
            string Description = "";
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    Description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            if (Description == "EmployeeInformation")
                return 1;
            if (Description == "Att.Detail")
                return 2;
            if (Description == "Increment Detail")
                return 3;
            if (Description == "Bank Detail")
                return 4;
            else
                return 0;

        }

        public ActionResult ViewSalarySheetPropertys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            string sSQL = "Select * from SalarySheetProperty";
            _oSalarySheetPropertys = SalarySheetProperty.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var oSalarySheetFormatPropertys = Enum.GetValues(typeof(EnumSalarySheetFormatProperty)).Cast<EnumSalarySheetFormatProperty>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            oSalarySheetFormatPropertys = oSalarySheetFormatPropertys.Where(x => (EnumSalarySheetFormatProperty)Convert.ToInt32(x.Value) != EnumSalarySheetFormatProperty.None).ToList();
            var objs = oSalarySheetFormatPropertys.Where(x => !_oSalarySheetPropertys.Select(p => p.SalarySheetFormatProperty).Contains((EnumSalarySheetFormatProperty)Convert.ToInt32(x.Value)));

            if (objs.Count() > 0)
            {
                foreach (var obj in objs)
                {
                    _oSalarySheetProperty = new SalarySheetProperty();
                    _oSalarySheetProperty.SalarySheetPropertyID = 0;
                    _oSalarySheetProperty.SalarySheetFormatProperty = (EnumSalarySheetFormatProperty)Convert.ToInt16(obj.Value);
                    _oSalarySheetProperty.PropertyFor = GetDescription(_oSalarySheetProperty.SalarySheetFormatProperty);
                    _oSalarySheetProperty.IsActive = true;
                    _oSalarySheetProperty = _oSalarySheetProperty.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oSalarySheetProperty.SalarySheetPropertyID > 0)
                    {
                        _oSalarySheetPropertys.Add(_oSalarySheetProperty);
                    }
                }
            }


            //string sSQL = "TRUNCATE TABLE SalarySheetProperty";
            //SalarySheetProperty.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //var oSalarySheetFormatPropertys = Enum.GetValues(typeof(EnumSalarySheetFormatProperty)).Cast<EnumSalarySheetFormatProperty>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //var objs = oSalarySheetFormatPropertys.Where(x => (EnumSalarySheetFormatProperty)Convert.ToInt32(x.Value) != EnumSalarySheetFormatProperty.None).ToList();

            //if (objs.Count() > 0)
            //{
            //    foreach (var obj in objs)
            //    {
            //        _oSalarySheetProperty = new SalarySheetProperty();
            //        _oSalarySheetProperty.SalarySheetPropertyID = 0;
            //        _oSalarySheetProperty.SalarySheetFormatProperty = (EnumSalarySheetFormatProperty)Convert.ToInt16(obj.Value);
            //        _oSalarySheetProperty.PropertyFor = GetDescription(_oSalarySheetProperty.SalarySheetFormatProperty);
            //        _oSalarySheetProperty.IsActive = true;
            //        _oSalarySheetProperty = _oSalarySheetProperty.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //        if (_oSalarySheetProperty.SalarySheetPropertyID > 0)
            //        {
            //            _oSalarySheetPropertys.Add(_oSalarySheetProperty);
            //        }
            //    }
            //}

            return View(_oSalarySheetPropertys);
        }

        [HttpPost]
        public JsonResult ActivityChange(SalarySheetProperty oSalarySheetProperty)
        {
            try
            {
                if (oSalarySheetProperty.SalarySheetPropertyID <= 0)
                    throw new Exception("Please select a valid item.");

                oSalarySheetProperty.IsActive = !oSalarySheetProperty.IsActive;
                oSalarySheetProperty = oSalarySheetProperty.IUD((int)EnumDBOperation.Active, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oSalarySheetProperty = new SalarySheetProperty();
                oSalarySheetProperty.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySheetProperty);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Salary Sheet Signature

        public ActionResult ViewSalarySheetSignatures(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();
            string sSQL = "Select * from SalarySheetSignature";
            oSalarySheetSignatures = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oSalarySheetSignatures);
        }

        [HttpPost]
        public JsonResult SaveSalarySheetSignature(SalarySheetSignature oSalarySheetSignature)
        {
            try
            {
                if (oSalarySheetSignature.SignatureID <= 0)
                {
                    oSalarySheetSignature = oSalarySheetSignature.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oSalarySheetSignature = oSalarySheetSignature.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oSalarySheetSignature = new SalarySheetSignature();
                oSalarySheetSignature.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySheetSignature);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSalarySheetSignature(SalarySheetSignature oSalarySheetSignature)
        {
            try
            {
                if (oSalarySheetSignature.SignatureID <= 0) { throw new Exception("Please select an valid item."); }
                oSalarySheetSignature = oSalarySheetSignature.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSalarySheetSignature = new SalarySheetSignature();
                oSalarySheetSignature.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySheetSignature.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
