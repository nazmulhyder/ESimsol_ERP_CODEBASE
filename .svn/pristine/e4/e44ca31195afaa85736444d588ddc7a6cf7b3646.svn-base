using ESimSol.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class SalaryFieldSetupController : Controller
    {
        #region Declaration 
        SalaryFieldSetup _oSalaryFieldSetup = new SalaryFieldSetup();
        List<SalaryFieldSetup> _oSalaryFieldSetups = new List<SalaryFieldSetup>();
        #endregion
        public ActionResult ViewSalaryFieldSetups(int MenuId)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, MenuId);

            _oSalaryFieldSetups = SalaryFieldSetup.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oSalaryFieldSetups);
        }

        public ActionResult ViewSalaryFieldSetup(int id)
        {
            SalaryFieldSetup _oSalaryFieldSetup = new SalaryFieldSetup();
            if (id > 0)
            {
                _oSalaryFieldSetup = _oSalaryFieldSetup.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<SalaryFieldSetupDetail> oSalaryFieldSetupDetails = new List<SalaryFieldSetupDetail>();
                oSalaryFieldSetupDetails = SalaryFieldSetupDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                _oSalaryFieldSetup.SalaryFieldSetupDetails = oSalaryFieldSetupDetails;
            }
            ViewBag.PageOrientations = EnumObject.jGets(typeof(EnumPageOrientation));
            return View(_oSalaryFieldSetup);
        }

        [HttpPost]
        public JsonResult Save(SalaryFieldSetup oSalaryFieldSetup)
        {
            _oSalaryFieldSetup = new SalaryFieldSetup();
            try
            {
                _oSalaryFieldSetup = oSalaryFieldSetup;
                _oSalaryFieldSetup = _oSalaryFieldSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSalaryFieldSetup = new SalaryFieldSetup();
                _oSalaryFieldSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryFieldSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SalaryFieldSetup oSalaryFieldSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSalaryFieldSetup.Delete(oSalaryFieldSetup.SalaryFieldSetupID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsSalaryBySalaryField()
        {
            List<EnumObject> _oEnumObjects = new List<EnumObject>();

            _oEnumObjects = EnumObject.jGets(typeof(EnumSalaryField)).Where(x=>x.id>0).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEnumObjects);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}