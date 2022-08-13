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
    public class ImportProductController : Controller
    {
        #region Declaration
        ImportProduct _oImportProduct = new ImportProduct();
        List<ImportProduct> _oImportProducts = new List<ImportProduct>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
       
        #endregion

        public ActionResult ViewImportProducts(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oImportProduct = new ImportProduct();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
                _oImportProducts = ImportProduct.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oImportProducts = ImportProduct.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            
            
            return View(_oImportProducts);
        }
        public ActionResult ViewImportProduct(int id)
        {
            _oImportProduct = _oImportProduct.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oImportProduct = new ImportProduct();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            _oImportProduct = _oImportProduct.Get(id,((User)Session[SessionInfo.CurrentUser]).UserID);
            _oImportProduct.ImportProductDetails = ImportProductDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProductNature = EnumObject.jGets(typeof(EnumProductNature));

            return View(_oImportProduct);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(ImportProduct oImportProduct)
        {
            _oImportProduct = new ImportProduct();
            try
            {
                _oImportProduct = oImportProduct;
                _oImportProduct = _oImportProduct.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oImportProduct.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        [HttpPost]
        public JsonResult Activate_Setup(ImportProduct oImportProduct)
        {
            _oImportProduct = new ImportProduct();
            string sMsg = "";
            _oImportProduct = oImportProduct.Activate(((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oImportLetterSetup.ErrorMessage = sMsg;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportProduct);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region HTTP Delete

        [HttpPost]
        public JsonResult Delete(ImportProduct oImportProduct)
        {
            string sErrorMease = "";
            try
            {

                sErrorMease = oImportProduct.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     
        #endregion

     


    }
}
