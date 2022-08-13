using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class ProductSetupController : Controller
    {
        #region Declaration
        ProductSetup _oProductSetup = new ProductSetup();
        List<ProductSetup> _oProductSetups = new List<ProductSetup>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
       
        #endregion

        public ActionResult ViewProductSetup(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductSetup = new ProductSetup();
            _oProductSetup = _oProductSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oProductSetup);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(ProductSetup oProductSetup)
        {
            _oProductSetup = new ProductSetup();
            try
            {
                _oProductSetup = oProductSetup;
                _oProductSetup = _oProductSetup.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oProductSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                ProductSetup oProductSetup = new ProductSetup();
                smessage = oProductSetup.Delete(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        [HttpPost]
        public JsonResult GetByCategory(ProductSetup oProductSetup)
        {
            _oProductSetup = new ProductSetup();
            try
            {
                _oProductSetup = _oProductSetup.GetByCategory(oProductSetup.ProductCategoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oProductSetup = new ProductSetup();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        


    }
}
