using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class DyeingCapacityController : Controller
    {
        #region Declaration
        DyeingCapacity _oDyeingCapacity = new DyeingCapacity();
        List<DyeingCapacity> _oDyeingCapacitys = new List<DyeingCapacity>();
        string _sErrorMessage = "";
      
        #endregion

        #region Functions


        #region Actions
        public ActionResult ViewDyeingCapacitys(int buid,int menuid)
        {
            try
            {
                this.Session.Remove(SessionInfo.MenuID);
                this.Session.Add(SessionInfo.MenuID, menuid);
                //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'DyeingCapacity', 'DyeingCapacityBranch'", (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
                 
                _oDyeingCapacitys = new List<DyeingCapacity>();
                _oDyeingCapacitys = DyeingCapacity.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.BUID = buid;
                return View(_oDyeingCapacitys);
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "User");
            }
        }
        public ActionResult ViewDyeingCapacity(int id,int buid)
        {
            _oDyeingCapacity = new DyeingCapacity();
            if (id > 0)
            {
                _oDyeingCapacity = _oDyeingCapacity.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.DyeingType = EnumObject.jGets(typeof(EumDyeingType));            
            ViewBag.MUnit = MeasurementUnit.Gets(EnumUniteType.Weight, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oDyeingCapacity);
        }
        
       
        [HttpPost]
        public JsonResult Save(DyeingCapacity oDyeingCapacity)
        {
            _oDyeingCapacity = new DyeingCapacity();
            try
            {
                _oDyeingCapacity = oDyeingCapacity;
                _oDyeingCapacity = _oDyeingCapacity.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingCapacity = new DyeingCapacity();
                _oDyeingCapacity.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingCapacity);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(DyeingCapacity oDyeingCapacity)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oDyeingCapacity.Delete(oDyeingCapacity.DyeingCapacityID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        public JsonResult Get(DyeingCapacity oDyeingCapacity)
        {
            _oDyeingCapacity = new DyeingCapacity();
            try
            {
                if (oDyeingCapacity.DyeingCapacityID > 0)
                {
                    _oDyeingCapacity = _oDyeingCapacity.Get(oDyeingCapacity.DyeingCapacityID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oDyeingCapacity = new DyeingCapacity();
                _oDyeingCapacity.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingCapacity);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

      

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion

        public ActionResult PrintList()
        {
            _oDyeingCapacitys = new List<DyeingCapacity>();
            _oDyeingCapacitys = DyeingCapacity.Gets((int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptDyeingCapacitys oReport = new rptDyeingCapacitys();
            byte[] abytes = oReport.PrepareReport(_oDyeingCapacitys, oCompany, "Dyeing Capacity List");
            return File(abytes, "application/pdf");
        }


        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                DyeingCapacity oDyeingCapacity = new DyeingCapacity();
                sFeedBackMessage = oDyeingCapacity.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

    }
}
