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
    public class SizeCategoryController : Controller
    {
        #region Yarn Requsition
        YarnRequisition _oYarnRequisition = new YarnRequisition();
        List<YarnRequisition> _oYarnRequisitions = new List<YarnRequisition>();
        YarnRequisitionDetail _oYarnRequisitionDetail = new YarnRequisitionDetail();
        List<YarnRequisitionDetail> _oYarnRequisitionDetails = new List<YarnRequisitionDetail>();  
        #endregion

        #region Declaration
        SizeCategory _oSizeCategory = new SizeCategory();
        List<SizeCategory> _oSizeCategorys = new List<SizeCategory>();
        string _sErrorMessage = "";
        #endregion

        #region Functions
        
        #endregion

        public ActionResult ViewSizeCategorys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SizeCategory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oSizeCategorys = new List<SizeCategory>();
            _oSizeCategorys = SizeCategory.Gets((int)Session[SessionInfo.currentUserID]);            
            return View(_oSizeCategorys);
        }

        public ActionResult ViewSizeCategory(int id)
        {
            _oSizeCategory = new SizeCategory();
            if (id > 0)
            {
                _oSizeCategory = _oSizeCategory.Get(id, (int)Session[SessionInfo.currentUserID]);
            }            
            return View(_oSizeCategory);
        }

        [HttpPost]
        public JsonResult Save(SizeCategory oSizeCategory)
        {
            _oSizeCategory = new SizeCategory();
            try
            {
                _oSizeCategory = oSizeCategory;                
                _oSizeCategory = _oSizeCategory.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSizeCategory = new SizeCategory();
                _oSizeCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSizeCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetSequence(SizeCategory oSizeCategory)
        {
            _oSizeCategorys = new List<SizeCategory>();
            try
            {
                _oSizeCategorys = SizeCategory.ResetSequence(oSizeCategory, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSizeCategorys = new List<SizeCategory>();
                _oSizeCategory = new SizeCategory();
                _oSizeCategory.ErrorMessage = ex.Message;
                _oSizeCategorys.Add(_oSizeCategory);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSizeCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                SizeCategory oSizeCategory = new SizeCategory();
                sFeedBackMessage = oSizeCategory.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Searching
        public ActionResult SizeCategorySearch()
        {
            _oSizeCategorys = new List<SizeCategory>();            
            return PartialView(_oSizeCategorys);
        }
        
       
        
        [HttpPost]
        public JsonResult Gets()
        {
            List<SizeCategory> oSizeCategorys = new List<SizeCategory>();
            oSizeCategorys = SizeCategory.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oSizeCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchSize(SizeCategory oSizeCategory)
        {
            List<SizeCategory> oSizeCategorys = new List<SizeCategory>();
            if (oSizeCategory.Param == null || oSizeCategory.Param =="")
            {
                oSizeCategorys = SizeCategory.Gets((int)Session[SessionInfo.currentUserID]);
            }else
            {
                oSizeCategorys = SizeCategory.GetsBySizeCategory(oSizeCategory.Param, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSizeCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
            
        }
        #endregion

        #region Print List
        public ActionResult PrintList(string sIDs)
        {

            _oSizeCategory = new SizeCategory();
            string sSQL = "SELECT * FROM SizeCategory WHERE SizeCategoryID IN (" + sIDs + ") ORDER BY Sequence ASC";
            _oSizeCategory.SizeCategories = SizeCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptSizeCategoryList oReport = new rptSizeCategoryList();
            byte[] abytes = oReport.PrepareReport(_oSizeCategory, oCompany);
            return File(abytes, "application/pdf");
        }


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

    }
}
