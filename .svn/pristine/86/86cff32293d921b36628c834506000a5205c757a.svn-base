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
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
    public class ColorCategoryController : Controller
    {
        #region Declaration
        ColorCategory _oColorCategory = new ColorCategory();
        List<ColorCategory> _oColorCategorys = new List<ColorCategory>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewColorCategorys(int menuid)
        {
            
	        this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ColorCategory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oColorCategorys = new List<ColorCategory>();
            _oColorCategorys = ColorCategory.Gets((int)Session[SessionInfo.currentUserID]);

            List<AutoComplete> oAutoCompletes = new List<AutoComplete>();
            AutoComplete oAutoComplete = new AutoComplete();
            foreach (ColorCategory oItem in _oColorCategorys)
            {
                oAutoComplete = new AutoComplete();
                oAutoComplete.data = oItem.ColorCategoryID.ToString();
                oAutoComplete.value = oItem.ColorName;
                oAutoCompletes.Add(oAutoComplete);
            }

            ViewBag.ColorCategorys = oAutoCompletes;
            return View(_oColorCategorys);
        }

        public ActionResult ViewColorCategory(int id)
        {
            _oColorCategory = new ColorCategory();
            if (id > 0)
            {
                _oColorCategory = _oColorCategory.Get(id, (int)Session[SessionInfo.currentUserID]);
            }            
            return View(_oColorCategory);
        }

        [HttpPost]
        public JsonResult Save(ColorCategory oColorCategory)
        {
            _oColorCategory = new ColorCategory();
            try
            {
                _oColorCategory = oColorCategory;                
                _oColorCategory = _oColorCategory.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oColorCategory = new ColorCategory();
                _oColorCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oColorCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ColorCategory oColorCategory = new ColorCategory();
                sFeedBackMessage = oColorCategory.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchColor(string sTemp, double ts)
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            oColorCategorys = ColorCategory.GetsbyColorName(sTemp, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oColorCategorys);
        }

        public ActionResult ColorCategorySearch()
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();            
            return PartialView(oColorCategorys);
        }
        #region Searching

        [WebMethod]
        public List<AutoComplete> GetsForAutoComplete()
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            oColorCategorys = ColorCategory.Gets((int)Session[SessionInfo.currentUserID]);

            List<AutoComplete> oAutoCompletes = new List<AutoComplete>();
            AutoComplete oAutoComplete = new AutoComplete();
            foreach (ColorCategory oItem in oColorCategorys)
            {
                oAutoComplete = new AutoComplete();
                oAutoComplete.value = oItem.ColorCategoryID.ToString();
                oAutoComplete.data = oItem.ColorName;
                oAutoCompletes.Add(oAutoComplete);
            }
            return oAutoCompletes;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize((object)oAutoCompletes);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
     
        [HttpPost]
        public JsonResult Gets()
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            oColorCategorys = ColorCategory.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oColorCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SearchColor(ColorCategory oColorCategory)
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            if(oColorCategory.Param==null|| oColorCategory.Param=="")
            {
                oColorCategorys = ColorCategory.Gets((int)Session[SessionInfo.currentUserID]);
            }
            {
                oColorCategorys = ColorCategory.GetsbyColorName(oColorCategory.Param, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oColorCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getColorByColorName(ColorCategory oColorCategory)
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            
            {
                oColorCategorys = ColorCategory.GetsbyColorName(oColorCategory.ColorName, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oColorCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print List
        public ActionResult PrintList(string sIDs)
        {

            _oColorCategory = new ColorCategory();
            string sSQL = "SELECT * FROM ColorCategory WHERE ColorCategoryID IN (" + sIDs + ")";
            _oColorCategory.ColorCategories = ColorCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptColorCategoryList oReport = new rptColorCategoryList();
            byte[] abytes = oReport.PrepareReport(_oColorCategory, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ColorCategoryExportToExcel(string sIDs)
        {

            _oColorCategory = new ColorCategory();
            string sSQL = "SELECT * FROM ColorCategory WHERE ColorCategoryID IN (" + sIDs + ")";
            _oColorCategory.ColorCategories = ColorCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;
            rptColorCategoryList oReport = new rptColorCategoryList();
            PdfPTable oPdfPTable = oReport.PrepareExcel(_oColorCategory, oCompany);

            ExportToExcel.WorksheetName = "Color Category";
            byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

            Response.ClearContent();
            Response.BinaryWrite(abytes);
            Response.AddHeader("content-disposition", "attachment; filename=ColorCategory.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
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

    public class AutoComplete
    {
        public AutoComplete() {
            value = "";
            data = "";
        }
        public string value { get; set; }
        public string data { get; set; }
    }
}
