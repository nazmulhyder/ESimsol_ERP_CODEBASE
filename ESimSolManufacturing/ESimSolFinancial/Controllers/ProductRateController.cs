using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class ProductRateController : Controller
    {
        #region Declaration
        List<ProductRate> _oProductRates = new List<ProductRate>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewProductRates(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'ProductBase', 'Property'", (int)Session[SessionInfo.currentUserID], (Guid)Session[SessionInfo.wcfSessionID]));

            _oProductRates = new List<ProductRate>();
            _oProductRates = ProductRate.Gets((int)Session[SessionInfo.currentUserID]);//need to get by BUID
            ViewBag.ProductRates = _oProductRates;
            //ViewBag.ProductTypeObjs = ProductTypeObj.Gets();
            return View(_oProductRates);
        }

        [HttpPost]
        public JsonResult GetByProduct(Product oProduct)
        {
            _oProductRates = new List<ProductRate>();
            try
            {
                _oProductRates = ProductRate.Gets(oProduct.ProductID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductRates = new List<ProductRate>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductRates);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult Save(ProductRate oProductRate)
        {
            oProductRate.ActivationDate = DateTime.Now;
            oProductRate.SaleSchemeID = 0;
            oProductRate.Rate = Math.Round(Math.Abs(oProductRate.Rate), 2);
            try
            {
                oProductRate = oProductRate.Save((int)Session[SessionInfo.currentUserID]);
                _oProductRates = ProductRate.Gets(oProductRate.ProductID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductRate = new ProductRate();
                oProductRate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductRates);
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

    }
}