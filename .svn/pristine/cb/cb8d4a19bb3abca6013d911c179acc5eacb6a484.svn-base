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
    public class ModelCategoryController : Controller
    {
        #region Declaration
        ModelCategory _oModelCategory = new ModelCategory();
        List<ModelCategory> _oModelCategorys = new List<ModelCategory>();
        string _sErrorMessage = "";
        #endregion

        #region Functions

        #endregion

        public ActionResult ViewModelCategorys(int menuid)
        {
            
	        this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ModelCategory).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oModelCategorys = new List<ModelCategory>();
            _oModelCategorys = ModelCategory.Gets((int)Session[SessionInfo.currentUserID]);

            return View(_oModelCategorys);
        }

        public ActionResult ViewModelCategory(int id)
        {
            _oModelCategory = new ModelCategory();
            if (id > 0)
            {
                _oModelCategory = _oModelCategory.Get(id, (int)Session[SessionInfo.currentUserID]);
            }            
            return View(_oModelCategory);
        }

        [HttpPost]
        public JsonResult Save(ModelCategory oModelCategory)
        {
            _oModelCategory = new ModelCategory();
            try
            {
                _oModelCategory = oModelCategory;                
                _oModelCategory = _oModelCategory.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oModelCategory = new ModelCategory();
                _oModelCategory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oModelCategory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ModelCategory oModelCategory = new ModelCategory();
                sFeedBackMessage = oModelCategory.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
            List<ModelCategory> oModelCategorys = new List<ModelCategory>();
            oModelCategorys = ModelCategory.GetsbyCategoryName(sTemp, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oModelCategorys);
        }

        public ActionResult ModelCategorySearch()
        {
            List<ModelCategory> oModelCategorys = new List<ModelCategory>();            
            return PartialView(oModelCategorys);
        }
        #region Searching

        [WebMethod]
        public List<AutoComplete> GetsForAutoComplete()
        {
            List<ModelCategory> oModelCategorys = new List<ModelCategory>();
            oModelCategorys = ModelCategory.Gets((int)Session[SessionInfo.currentUserID]);

            List<AutoComplete> oAutoCompletes = new List<AutoComplete>();
            AutoComplete oAutoComplete = new AutoComplete();
            foreach (ModelCategory oItem in oModelCategorys)
            {
                oAutoComplete = new AutoComplete();
                oAutoComplete.value = oItem.ModelCategoryID.ToString();
                oAutoComplete.data = oItem.CategoryName;
                oAutoCompletes.Add(oAutoComplete);
            }
            return oAutoCompletes;

         
        }
        
     
        [HttpPost]
        public JsonResult Gets()
        {
            List<ModelCategory> oModelCategorys = new List<ModelCategory>();
            oModelCategorys = ModelCategory.Gets((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oModelCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SearchColor(ModelCategory oModelCategory)
        {
            List<ModelCategory> oModelCategorys = new List<ModelCategory>();
            if(oModelCategory.Param==null|| oModelCategory.Param=="")
            {
                oModelCategorys = ModelCategory.Gets((int)Session[SessionInfo.currentUserID]);
            }
            {
                oModelCategorys = ModelCategory.GetsbyCategoryName(oModelCategory.Param, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oModelCategorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print List
        public ActionResult PrintList(string sIDs)
        {

            _oModelCategory = new ModelCategory();
            string sSQL = "SELECT * FROM ModelCategory WHERE ModelCategoryID IN (" + sIDs + ")";
            _oModelCategory.ColorCategories = ModelCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptModelCategoryList oReport = new rptModelCategoryList();
            byte[] abytes = oReport.PrepareReport(_oModelCategory, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ModelCategoryExportToExcel(string sIDs)
        {

            _oModelCategory = new ModelCategory();
            string sSQL = "SELECT * FROM ModelCategory WHERE ModelCategoryID IN (" + sIDs + ")";
            _oModelCategory.ColorCategories = ModelCategory.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null;
            rptModelCategoryList oReport = new rptModelCategoryList();
            PdfPTable oPdfPTable = oReport.PrepareExcel(_oModelCategory, oCompany);

            ExportToExcel.WorksheetName = "Color Category";
            byte[] abytes = ExportToExcel.ConvertToExcel(oPdfPTable);

            Response.ClearContent();
            Response.BinaryWrite(abytes);
            Response.AddHeader("content-disposition", "attachment; filename=ModelCategory.xlsx");
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

   
}
