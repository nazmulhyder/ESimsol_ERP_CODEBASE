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
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class BrandController : Controller
    {
        #region Declaration
        Brand _oBrand = new Brand();
        List<Brand> _oBrands = new List<Brand>();
        ContactPersonnel _oContactPersonnel = new ContactPersonnel();
        List<ContactPersonnel> _oContactPersonnels = new List<ContactPersonnel>();
        BuyerConcern _oBuyerConcern = new BuyerConcern();
        List<BuyerConcern> _oBuyerConcerns = new List<BuyerConcern>();        
        List<BuyerWiseBrand> _oBuyerWiseBrands = new List<BuyerWiseBrand>();
        #endregion

        public ActionResult ViewBrands(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Brand).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oBrands = new List<Brand>();
            _oBrands = Brand.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oBrands);
        }

        public ActionResult ViewBrand(int id, double ts)
        {
            _oBrand = new Brand();
            if(id>0)
            {
                _oBrand = _oBrand.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oBrand);
        }

        [HttpPost]
        public JsonResult Save(Brand oBrand)
        {
            _oBrand = new Brand();
            try
            {
                _oBrand = oBrand;
                _oBrand = _oBrand.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBrand.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBrand);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            Brand oBrand = new Brand();
            try
            {
                sErrorMease = oBrand.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print
        //public ActionResult PrintList(string sIDs)
        //{

        //    _oBrand = new Brand();
        //    string sSQL = "SELECT * FROM Brand WHERE BrandID IN (" + sIDs + ")";
        //    _oBrand.Brands = Brand.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptBrandList oReport = new rptBrandList();
        //    byte[] abytes = oReport.PrepareReport(_oBrand, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Assign  Buyers
        [HttpPost]
        public JsonResult AssignBuyers(Brand oBrand) 
        {
            _oBrand = new Brand();
            try
            {
                _oBrand.BrandID = oBrand.BrandID;
                if (oBrand.BrandID > 0)
                {
                    _oBrand.BuyerWiseBrands = BuyerWiseBrand.GetsByBrand(_oBrand.BrandID, (int)Session[SessionInfo.currentUserID]);
                }
                _oBrand.Buyers = Contractor.GetsByNamenType("", Convert.ToString((int)EnumContractorType.Buyer), 0, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBrand = new Brand();
                _oBrand.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBrand);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AssignBrands(Brand oBrand)
        {
            _oBrand = new Brand();
            try
            {
                _oBrand.BuyerID = oBrand.BuyerID;
                if (oBrand.BuyerID > 0)
                {
                    _oBrand.BuyerWiseBrands = BuyerWiseBrand.GetsByBuyer(_oBrand.BuyerID, (int)Session[SessionInfo.currentUserID]);
                }
                _oBrand.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBrand = new Brand();
                _oBrand.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBrand);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region Commit Buyer wise Brand
        [HttpPost]
        public JsonResult CommitBuyerWiseBrand(BuyerWiseBrand oBuyerWiseBrand)
        {
            string sfeedBackMessage = "";
            try
            {
                sfeedBackMessage = oBuyerWiseBrand.Save((int)Session[SessionInfo.currentUserID], oBuyerWiseBrand, oBuyerWiseBrand.IsShortList, oBuyerWiseBrand.IsBuyerBased);
            }
            catch (Exception ex)
            {
                _oBrand = new Brand();
                _oBrand.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sfeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region Searching
        public ActionResult BrandSearch()
        {
            List<Brand> oBrands = new List<Brand>();
            return PartialView(oBrands);
        }
        
        [HttpGet]
        public JsonResult GetBrands(int nBuyerID, double ts)
        {
            _oBuyerWiseBrands = new List<BuyerWiseBrand>();
            try
            {
                _oBuyerWiseBrands = BuyerWiseBrand.GetsByBuyer(nBuyerID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                BuyerWiseBrand oBuyerWiseBrand = new BuyerWiseBrand();
                oBuyerWiseBrand.ErrorMessage = ex.Message;
                _oBuyerWiseBrands.Add(oBuyerWiseBrand);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBuyerWiseBrands);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(Brand oBrand)
        {
            _oBrand = new Brand();
            try
            {
                if (oBrand.BrandID <= 0) { throw new Exception("Please select a valid Brand."); }
                _oBrand = _oBrand.Get(oBrand.BrandID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBrand.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBrand);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

     

        [HttpGet]
        public string BrandSearchByName(string Name)
        {
            List<Brand> oBrands = new List<Brand>();
            oBrands = Brand.GetsByName(Name, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBrands);
            return sjson;
        }
        

      
        #endregion


      
        

      
    }
}
