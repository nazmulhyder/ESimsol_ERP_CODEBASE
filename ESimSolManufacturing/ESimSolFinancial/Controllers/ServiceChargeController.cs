using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class ServiceChargeController : PdfViewController
    {
        #region Declaration
        ServiceCharge _oServiceCharge;
        List<ServiceCharge> _oServiceCharges;
        
        #endregion

        #region Views
        public ActionResult ViewServiceCharges(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oServiceCharges = new List<ServiceCharge>();
            _oServiceCharges = ServiceCharge.Gets("SELECT * FROM ServiceCharge",  ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oServiceCharges);
        }
        public ActionResult ViewServiceCharge(int id)
        {
            _oServiceCharge = new ServiceCharge();
            if (id > 0)
            {
                _oServiceCharge = ServiceCharge.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return View(_oServiceCharge);
        }

        #endregion

        #region IUD ServiceCharge
        [HttpPost]
        public JsonResult ServiceCharge_IU(ServiceCharge oServiceCharge)
        {
            _oServiceCharge = new ServiceCharge();
            try
            {
                _oServiceCharge = oServiceCharge;
                if (_oServiceCharge.ServiceChargeID > 0)
                {
                    _oServiceCharge = _oServiceCharge.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oServiceCharge = _oServiceCharge.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oServiceCharge = new ServiceCharge();
                _oServiceCharge.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceCharge);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ServiceCharge_Delete(ServiceCharge oServiceCharge)
        {
            _oServiceCharge = new ServiceCharge();
            List<ServiceCharge> oServiceCharges = new List<ServiceCharge>();
            try
            {
                _oServiceCharge = oServiceCharge.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                _oServiceCharge = new ServiceCharge();
                _oServiceCharge.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceCharge);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion




    }
}
